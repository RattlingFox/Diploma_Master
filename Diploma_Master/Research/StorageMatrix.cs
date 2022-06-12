using Diploma_Master.Objects;
using System;
using System.Collections.Generic;

namespace Diploma_Master
{
    internal static class StorageMatrix
    {

        // Генерация начального решения
        // Иницилизация матрицы хранения фрагментов файлов на узлах в хранилище
        // На вход запрашиваем инициализированный экземпляр storage(хранилище) с заполненными параметрами
        // и количество фрагментов у каждого файла (предполагается, что генерация фрагментов файлов и матрицы хранения
        // будет происходить по одним и тем же параметрам)
        // Проверка на соответствие решения ограничениям выполняется снаружи
        public static SolutionObject InitStorageMatrix(InitObject storage, int gens)
        {
            var result = new SolutionObject()
            {
                FileStorageMatrix = new int[storage.FileCount, gens],
                DistributedFiles = new List<int>()
            };

            Random rnd = new();

            for (int i = 0; i <= storage.FileCount; i++)
            {
                for (int j = 0; j <= gens; j++)
                {
                    result.FileStorageMatrix[i, j] = rnd.Next(0, storage.FileCount);

                    // Проверка на 0 в матрице хранения, в случае нераспределения хотябы одного из фрагментов файла,
                    // файл исключается из хранилища и текущей популяции
                    if (result.FileStorageMatrix[i, j] == 0 && result.DistributedFiles.Contains(i))
                    {
                        result.DistributedFiles[result.DistributedFiles.Count] = i;
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