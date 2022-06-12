using Diploma_Master.Objects;
using System;
using System.Collections.Generic;

namespace Diploma_Master
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
            var result = new SolutionObject()
            {
                FileStorageMatrix = new int[storage.HiveCount,storage.FileCount, gens],
                DistributedFiles = new List<int>()
            };

            Random rnd = new();

            for (int i = 0; i <= storage.FileCount; i++)
            {
                for (int j = 0; j <= gens; j++)
                {
                    int temp = rnd.Next(0, storage.HiveCount);
                    result.FileStorageMatrix[temp, i, j] = 1;

                    // Проверка на 0 в матрице хранения, в случае нераспределения хотябы одного из фрагментов файла,
                    // файл исключается из хранилища и текущей популяции
                    var check = true;
                    if (result.FileStorageMatrix[temp, i, j] == 0)
                    {
                        check = false;
                    }
                    if (check == true && j == gens)
                    {
                        result.DistributedFiles.Add(i);
                    }
                }
            }

            // Перебор файлов в хранилище для подсчёта суммарного объёма
            foreach (Files j in storage.Files)
            {
                for (int num = 0; num <= j.fileNumber; num++)
                {
                    if (result.DistributedFiles.Contains(num))
                    {
                        result.FileSizeSum += j.fileSize;
                    };
                }
            }

            return result;

        }
    }
}