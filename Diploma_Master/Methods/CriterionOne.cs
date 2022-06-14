using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Methods
{
    internal static class CriterionOne
    {
        /// <summary>
        /// Расчёт критерия К1 по формуле (2.2). Суммирование для каждого узла хранилища разности объёма узла и двойной суммы
        /// произведения компонент матрицы объёмов фрагментов файлов на компоненты матрицы хранения фрагментов файлов, хранимых на узле.
        /// На вход запрашиваются инициализированные объекты "Хранилище" и "Решение"
        /// На выходе дробное значение критерия К1
        /// </summary>
        /// <param name="storage"> Инициализированный обект "Хранилище" </param>
        /// <param name="solution"> Инициализированый объект "Решение" </param>
        /// <returns></returns>
        public static float CriterionOneCalc(StorageObject storage, SolutionObject solution, int gens)
        {
            float result = 0;

            for(int h = 0; h < storage.HiveCount; h++)
            {
                int filesSize = 0;
                foreach (Files j in storage.Files)
                {
                    if (solution.DistributedFiles.Contains(j.fileNumber))
                    {
                        for (int i = 0; i < j.fileFragmentsSize.Length; i++)
                        {
                            filesSize += i * StorageMatrix.StorageMatrixToArray(storage, solution, gens)[h, j.fileNumber, i];
                        }
                    }                    
                }
                result += storage.HivesSize[h] - filesSize;
            }

            return result;
        }
    }
}
