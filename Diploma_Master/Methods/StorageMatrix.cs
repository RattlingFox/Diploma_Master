using Diploma_Master.Objects;
using System;
using System.Collections.Generic;

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
            Random rnd = new();

            var solution = new SolutionObject()
            {
                FileStorageMatrix = new int[storage.HiveCount, storage.FileCount, gens],
                DistributedFiles = new List<int>(),
                FileSizeSum = 0
            };

            for (int i = 0; i < storage.FileCount; i++) 
            {
                for (int j = 0; j < gens; j++)
                {
                    int temp = rnd.Next(0, storage.HiveCount);
                    solution.FileStorageMatrix[temp, i, j] = 1;
                }
            }

            for (int q = 0; q < storage.HiveCount; q++) 
            {
                for (int i = 0; i < storage.FileCount; i++)
                {
                    for (int j = 0; j < gens; j++)
                    {
                        // Проверка на 0 в матрице хранения, в случае нераспределения хотябы одного из фрагментов файла,
                        // файл исключается из хранилища и текущей популяции
                        var check = true;
                        if (solution.FileStorageMatrix[q, i, j] == 0)
                        {
                            check = false;
                        }
                        if (check == true && j == gens-1)
                        {
                            solution.DistributedFiles.Add(i);
                        }
                    }
                }
            }

            // Перебор файлов в хранилище для подсчёта суммарного объёма
            foreach (Files j in storage.Files)
            {
                for (int num = 0; num < j.fileNumber; num++)
                {
                    if (solution.DistributedFiles.Contains(num))
                    {
                        solution.FileSizeSum += j.fileSize;
                    };
                }
            }

            return solution;
        }
        /// <summary>
        /// Метод для пересчёта параметров нового решения. Пересчитывает список полностью распределённых файлов по узлам и их общий объём.
        /// На вход метод запрашивает экземпляр "Хранилища", количество частеЙ, на который делиться каждый файл и экземпляр "решения" для пересчёта
        /// с новой матрицей хранения.
        /// На выходе метод отдаёт экземпляр "Решения" с новыми значениями параметров.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище" </param>
        /// <param name="gens"> Количество частей, на которые делится каждый файл </param>
        /// <returns></returns>
        public static SolutionObject RecalcSolution(StorageObject storage, int gens, SolutionObject solution)
        {
            solution.DistributedFiles.Clear();

            for (int q = 0; q < storage.HiveCount; q++)
            {
                for (int i = 0; i < storage.FileCount; i++)
                {
                    for (int j = 0; j <= gens; j++)
                    {
                        // Проверка на 0 в матрице хранения, в случае нераспределения хотябы одного из фрагментов файла,
                        // файл исключается из хранилища и текущей популяции
                        var check = true;
                        if (solution.FileStorageMatrix[q, i, j] == 0)
                        {
                            check = false;
                        }
                        if (check == true && j == gens)
                        {
                            solution.DistributedFiles.Add(i);
                        }
                    }
                }
            }

            // Перебор файлов в хранилище для подсчёта суммарного объёма
            foreach (Files j in storage.Files)
            {
                for (int num = 0; num < j.fileNumber; num++)
                {
                    if (solution.DistributedFiles.Contains(num))
                    {
                        solution.FileSizeSum += j.fileSize;
                    };
                }
            }

            return solution;
        }
    }
}