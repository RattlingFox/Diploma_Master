using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Research
{
    internal static class CriterionTwo
    {
        /// <summary>
        /// Расчёт по формуле для критерия К2
        /// Суммирование произведения компоненты матрицы использования файла на компоненту матрицы расстояний между узлами
        /// Результат есть сумма для всех файлов, распределённых в хранилище
        /// На вход запрашиваются инициализированные объекты storage (хранилище) и solution (решение)
        /// На выходе дробное значение критерия К2
        /// </summary>
        /// <param name="storage"> Инициализированный обект storage (хранилище) </param>
        /// <param name="solution"> Инициализированый объект solution (решение) </param>
        /// <returns></returns>
        public static float CriterionTwoCalc(InitObject storage, SolutionObject solution)
        {
            float result = 0;

            foreach (Files file in storage.Files)
            {
                if (solution.DistributedFiles.Contains(file.fileNumber))
                {
                    float unit = 0;
                    for (int j = 0; j <= storage.HiveCount; j++)
                    {
                        for (int q = 0; q <= storage.HiveCount; q++)
                        {
                            for (int i = 0; i <= file.fileFragmentsSize.Length; i++)
                            {
                                if (q == solution.FileStorageMatrix[file.fileNumber, i])
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
