﻿using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Methods
{
    internal static class CriterionTwo
    {
        /// <summary>
        /// Расчёт по формуле (2.3) для критерия К2
        /// Суммирование произведения компоненты матрицы использования файла на компоненту матрицы расстояний между узлами
        /// Результат есть сумма для всех файлов, распределённых в хранилище
        /// На вход запрашиваются инициализированные объекты "Хранилище" и "Решение"
        /// На выходе дробное значение критерия К2
        /// </summary>
        /// <param name="storage"> Инициализированный обект "Хранилище" </param>
        /// <param name="solution"> Инициализированый объект "Решение" </param>
        /// <returns></returns>
        public static float CriterionTwoCalc(StorageObject storage, SolutionObject solution)
        {
            float result = 0;

            foreach (Files file in storage.Files)
            {
                if (solution.DistributedFiles.Contains(file.fileNumber))
                {
                    float unit = 0;
                    for (int j = 0; j < storage.HiveCount; j++)
                    {
                        for (int q = 0; q < storage.HiveCount; q++)
                        {
                            for (int i = 0; i <= file.fileFragmentsSize.Length; i++)
                            {
                                if (1 == solution.FileStorageMatrix[q,file.fileNumber, i])
                                {
                                    unit += storage.FilesUsingMatrix[file.fileNumber, j] * storage.DistanceMatrix[q, j];
                                }
                            }
                        }
                    }
                    result += unit;
                }
            }

            return result;
        }
    }
}