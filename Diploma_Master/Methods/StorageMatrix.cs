using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diploma_Master.Methods
{
    internal static class StorageMatrix
    {
        /// <summary>
        /// Генерация начального решения.
        /// Иницилизация матрицы хранения фрагментов файлов на узлах в хранилище.
        /// На вход запрашиваем инициализированный экземпляр "Хранилище" с заполненными параметрами
        /// и количество фрагментов у каждого файла (предполагается, что генерация фрагментов файлов и матрицы хранения
        /// будет происходить по одним и тем же параметрам).
        /// Проверка на соответствие решения ограничениям выполняется снаружи
        /// </summary>
        /// <param name="storage"> Инициализированный обект "Хранилище" </param>
        /// <param name="gens"> Количество фрагментов, на которое делится каждый файл в хранилище </param>
        /// <returns></returns>
        public static SolutionObject InitStorageMatrix(StorageObject storage, int gens)
        {
            var rnd = new Random();
            int[] checkSize = new int[storage.NodeCount];

            var solution = new SolutionObject()
            {
                FileStorageMatrix = new List<List<int>>(),
                DistributedFiles = new List<int>(),
                FileSizeSum = 0
            };

            // Заполнение начального решения с ограничением до 5 фрагментов на 1 узел хранилища
            foreach (var j in storage.Files)
            {
               for (int i = 0; i < gens; i++)
                {
                    solution.FileStorageMatrix.Add(new List<int>());
                    var rndNum = rnd.Next(0, storage.NodeCount-1);               

                    while (solution.FileStorageMatrix.Any(x => x.Where(y => y == rndNum).Count() >= 5))
                    {
                        rndNum = 0;
                    }

                    solution.FileStorageMatrix[j.fileNumber][i] = rndNum;
                }                
            }

            // Перебор файлов в хранилище для формировани списка полностью распределённых файлов
            RecalcDistributedFiles(gens, solution);

            // Перебор файлов в хранилище для подсчёта суммарного объёма
            RecalcFileSizeSum(storage, solution);

            return solution;
        }

        /// <summary>
        /// Метод для пересчёта параметров нового решения. Пересчитывает список полностью распределённых файлов по узлам.
        /// На вход метод запрашивает количество частеЙ, на который делиться каждый файл и экземпляр "решения" для пересчёта с новой матрицей хранения.
        /// На выходе метод отдаёт экземпляр "Решения" с новыми значениями параметров.
        /// </summary>
        /// <param name="solution"> Экземпляр "Решение" </param>
        /// <param name="gens"> Количество частей, на которые делится каждый файл </param>
        /// <returns></returns>
        public static SolutionObject RecalcDistributedFiles(int gens, SolutionObject solution)
        {
            solution.DistributedFiles.Clear();

            int iter = 0;
            foreach (var j in solution.FileStorageMatrix)
            {
                int alpha = 0;
                foreach (var i in j)
                {
                    var check = true;
                    if (i == 0)
                    {
                        check = false;
                        break;
                    }
                    if (check == true && alpha == gens - 1)
                    {
                        solution.DistributedFiles.Add(iter);
                    }
                    alpha++;
                }

                iter++;
            }

            return solution;
        }

        /// <summary>
        /// Метод для пересчёта параметров нового решения. Пересчитывает общий объём полностью распределённых файлов по узлам.
        /// На вход метод запрашивает экземпляр "Хранилище" и экземпляр "решения" для пересчёта с новой матрицей хранения.
        /// На выходе метод отдаёт экземпляр "Решения" с новыми значениями параметров.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище" </param>
        /// <param name="solution"> Экземпляр "Решение" </param>
        /// <returns></returns>
        public static SolutionObject RecalcFileSizeSum(StorageObject storage, SolutionObject solution)
        { 
            // Перебор файлов в хранилище для подсчёта суммарного объёма
            foreach (Files j in storage.Files)
            {
                if (solution.DistributedFiles.Contains(j.fileNumber))
                {
                    solution.FileSizeSum += j.fileSize;
                };
            }

            return solution;
        }        

        /// <summary>
        /// Метод для получения матрицы хранения вида int[q,j,i] для расчёта критериев К1 и К2.
        /// На вход метод запрашивает экземпляр текущего "Хранилища", экземпляр текущего "Решения" и количество фрагментов каждого файла.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище" </param>
        /// <param name="solution"> Экземпляр "Решение" </param>
        /// <param name="gens"> Количество фрагментов каждого файла </param>
        /// <returns></returns>
        public static int[,,] StorageMatrixToArray(StorageObject storage, SolutionObject solution, int gens)
        {
            var result = new int[storage.NodeCount, storage.FileCount, gens];
            int iter = 0;

            foreach (var j in solution.FileStorageMatrix)
            {
                for (int q = 0; q < storage.NodeCount; q++)
                {
                    for (int i = 0; i < j.Count; i++)
                    {
                        if (j[i] == q)
                        {
                            result[q, iter, i] = 1;
                        }
                        else
                        {
                            result[q, iter, i] = 0;
                        }
                    }
                }
            }

            return result;
        }
    }
}