﻿using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var rnd = new Random();
            var populationNew = new List<PopulationObject>();
            var tempPopulation = new List<PopulationObject>();
            int iterThree = 0;

            tempPopulation.AddRange(populationOld.OrderBy(x => x.ObjectiveFunctionValue).Take(10));

            foreach (PopulationObject i in tempPopulation)
            {
                int iterTwo = 0;

                foreach (PopulationObject j in tempPopulation)
                {
                    if (i != j)
                    {
                        var rndNew = rnd.Next(1, storage.FileCount - 1);
                        int iterOne = 0;
                        var alpha = new PopulationObject() { Solution = new SolutionObject() };
                        populationNew.Add(alpha);
                        alpha.Solution.Files.Add(new Files());

                        for (var g = 0; g < storage.FileCount; g++)
                        {
                            if (iterOne < rndNew)
                            {
                                alpha.Solution.Files[iterOne]
                                    .fileFragmentsStorage.Add(
                                    i.Solution.Files[iterTwo]
                                    .fileFragmentsStorage[iterOne]
                                    );
                            }

                            if (iterOne >= rndNew)
                            {
                                alpha.Solution.Files[iterOne].fileFragmentsStorage.Add(j.Solution.Files[iterTwo].fileFragmentsStorage[iterOne]);
                            }
                        }

                        iterOne++;
                    }

                    iterTwo++;
                }
            }

            foreach (PopulationObject i in populationNew)
            {
                int q1 = rnd.Next(0, storage.FileCount);
                int q2 = rnd.Next(0, storage.FileCount);
                int q3 = rnd.Next(0, storage.FileCount);

                foreach (var j in i.Solution.Files)
                {
                    if (j.fileNumber == q1 || j.fileNumber == q2 || j.fileNumber == q3)
                    {
                        int h = rnd.Next(0, gens);
                        int t = rnd.Next(0, 1);
                        if (t == 0) { t = -1; }
                        if (j.fileFragmentsStorage[h] == 0) { t = 1; }
                        j.fileFragmentsStorage[h] += t;
                    }
                }
            }

            foreach (PopulationObject i in populationNew)
            {
                i.Solution = StorageMatrix.RecalcDistributedFiles(gens, i.Solution);
                i.Solution = StorageMatrix.RecalcFileSizeSum(storage, i.Solution);
            }

            foreach (PopulationObject i in populationNew)
            {
                i.GenerationNumber = populationOld[0].GenerationNumber + 1;
                i.IndividualNumber = iterThree++;
                i.ObjectiveFunctionValue = CriterionOne.CriterionOneCalc(storage, i.Solution, gens) + CriterionTwo.CriterionTwoCalc(storage, i.Solution, gens) +
                    CriterionThree.CriterionThreeCalc(i.Solution);
                i.ConstraintCheckResult = ConstraintsCheck.CheckSolution(storage, i.Solution, gens);

                if (i.ConstraintCheckResult == false)
                {
                    populationNew.Remove(i);
                }
            }

            return populationNew;
        }

        /// <summary>
        /// Метод инициализации начального поколения родителей из 10-ти популяций, каждая из которых соответствует ограничениям.
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
                var check = ConstraintsCheck.CheckSolution(storage, solution, gens);
                if (check == true)
                {
                    float result = CriterionOne.CriterionOneCalc(storage, solution, gens) +
                        CriterionTwo.CriterionTwoCalc(storage, solution, gens) + CriterionThree.CriterionThreeCalc(solution);

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
    }
}
