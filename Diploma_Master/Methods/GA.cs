using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Diploma_Master.Objects;

namespace Diploma_Master.Methods
{
    public class GA
    {
        private static readonly Random rnd = new();

        /// <summary>
        /// Реализация генетического алгоритма. Используется скрещивание топ-10 наилучших популяций из поколения родителей поочерёдно.
        /// Скрещивание происхордит по принципу половина от первого родителя и половина от второго, после чего вносится мутация в 3 случайных гена новой популяции
        /// на величину +1 или -1 к номеру узла, на котором распределён фрагмент файла.
        /// На вход метод требует экземпляр "Хранилище", список популяций поколения родителей и количество генов в каждом фрагменте хромосомы.
        /// На выходе метод отдаёт список популяций нового поколения.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище", для которого выполняется алгоритм </param>
        /// <param name="populationOld"> Поколение "родителей" </param>
        /// <param name="gens"> Количество генов во фрагменте хромосомы </param>
        /// <returns></returns>
        public static List<PopulationObject> GAStep(StorageObject storage, List<PopulationObject> populationOld, int gens)
        {
            var populationNew = new List<PopulationObject>();
            var tempPopulation = new List<PopulationObject>();
            int[,,] tempStorageMatrix = new int[storage.HiveCount, storage.FileCount, gens];
            int iter = 0;
            var alpha = new List<int[,,]>();


            tempPopulation.AddRange(populationOld.OrderBy(x => x.ObjectiveFunctionValue).Take(10));

            foreach (PopulationObject i in tempPopulation)
            {
                foreach (PopulationObject j in tempPopulation)
                {
                    var beta = new int[storage.HiveCount, storage.FileCount, gens];

                    if (i != j)
                    {
                        for (int g = 0; g < storage.FileCount; g++)
                        {
                            if (g < storage.FileCount / 2)
                            {
                                for (int h = 0; h < storage.HiveCount; h++)
                                {
                                    for (int p = 0; h < gens; h++)
                                    {
                                        beta[h, g, p] = i.Solution.FileStorageMatrix[h, g, p];
                                    }
                                }
                            }

                            else
                            {
                                for (int h = 0; h < storage.HiveCount; h++)
                                {
                                    for (int p = 0; h < gens; h++)
                                    {
                                        beta[h, g, p] = j.Solution.FileStorageMatrix[h, g, p];
                                    }
                                }
                            }
                        }
                    }

                    alpha.Add(beta);
                }
            }

            foreach (PopulationObject i in populationNew)
            {
                i.Solution.FileStorageMatrix = alpha[iter];
                iter++;
            }

            foreach (PopulationObject i in populationNew)
            {
                for (int j = 0; j < 3; j++)
                {
                    int q = rnd.Next(0, storage.HiveCount);
                    int p = rnd.Next(0, storage.FileCount);
                    int h = rnd.Next(0, gens);

                    int t = rnd.Next(0, 1);
                    if (t == 0) { t = -1; }

                    i.Solution.FileStorageMatrix[q, p, h] = 0;
                    i.Solution.FileStorageMatrix[q + t, p, h] = 0;
                }

                i.Solution = StorageMatrix.RecalcSolution(storage, gens, i.Solution);
            }

            iter = 0;
            foreach (PopulationObject i in populationNew)
            {
                i.GenerationNumber = populationOld[0].GenerationNumber + 1;
                i.IndividualNumber = iter;
                iter++;
                i.ObjectiveFunctionValue = CriterionOne.CriterionOneCalc(storage, i.Solution) + CriterionTwo.CriterionTwoCalc(storage, i.Solution) +
                    CriterionThree.CriterionThreeCalc(i.Solution);
                i.ConstraintCheckResult = ConstraintsCheck.CheckSolution(storage, i.Solution);

                if (i.ConstraintCheckResult == false)
                {
                    populationNew.Remove(i);
                }
            }

            return populationNew;
        }

        /// <summary>
        /// Метод инициализации начального поколения родителей из 50-ти популяций, каждая из которых соответствует ограничениям.
        /// На входе метод запрашивает экземпляр "Хранилище" и количество генов.
        /// На выходе метод отдаёт поколение в виде списка.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище", для которого выполняется алгоритм </param>
        /// <param name="gens"> Количество генов во фрагменте хромосомы </param>
        /// <returns></returns>
        public static List<PopulationObject> InitPopulation(StorageObject storage, int gens)
        {
            var population = new List<PopulationObject>();

            for (int i = 0; i < 10;)
            {
                var solution = StorageMatrix.InitStorageMatrix(storage, gens);
                var check = ConstraintsCheck.CheckSolution(storage, solution);
                if (check == true)
                {
                    float result = CriterionOne.CriterionOneCalc(storage, solution) +
                        CriterionTwo.CriterionTwoCalc(storage, solution) + CriterionThree.CriterionThreeCalc(solution);

                    population.Add(new PopulationObject
                    {
                        GenerationNumber = 0,
                        IndividualNumber = i,
                        Solution = solution,
                        ObjectiveFunctionValue = result,
                        ConstraintCheckResult = check
                    });

                    i++;
                }
            }

            return population;
        }

        /// <summary>
        /// Метод преобразует матрицу хранения в список, где индексом является номер файла, а содержимым одномерные массивы, 
        /// с индексами соответствующими номерам генов, а содержимым соответствующим номеру узла, на котором распределён фрагмент файла.
        /// На вход метод запрашивает экземпляр "Хранилища", экземпляр "Решения" и количество генов.
        /// На выходе метод отдаёт хромосому.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище", для которого выполняется алгоритм </param>
        /// <param name="solution"> Экземпляр "Решения" для преобразования вывода </param>
        /// <param name="gens"> Количество генов во фрагменте хромосомы </param>
        /// <returns></returns>
        public static List<int[]> StorageMatrixToList(StorageObject storage, SolutionObject solution, int gens)
        {
            var result = new List<int[]>();

            for (int j = 0; j < storage.FileCount; j++)
            {
                int[] temp = new int[gens];

                for (int q = 0; q < storage.HiveCount; q++)
                {
                    for (int i = 0; i <= gens; i++)
                    {
                        if (solution.FileStorageMatrix[q,j,i] == 1)
                        {
                            temp[i] = q;
                        }
                    }
                }

                result.Add(temp);
            }

            return result;
        }
    }
}
