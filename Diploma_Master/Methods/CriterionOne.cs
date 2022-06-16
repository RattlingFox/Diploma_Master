using Diploma_Master.Objects;

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
        /// <param name="gens"> Количество фрагментов каждого файла </param>
        /// <returns></returns>
        public static float CriterionOneCalc(StorageObject storage, SolutionObject solution, int gens)
        {
            float result = 0;

            for (int h = 0; h < storage.NodeCount; h++)
            {
                int filesSize = 0;
                foreach (var j in solution.Files)
                {
                    if (solution.DistributedFiles.Contains(j.fileNumber))
                    {
                        for (int i = 0; i < j.fileFragmentsSize.Count; i++)
                        {
                            filesSize += i * StorageMatrix.StorageMatrixToArray(storage, solution, gens)[h, j.fileNumber, i];
                        }
                    }
                }
                result += storage.NodesSize[h] - filesSize;
            }

            return result;
        }
    }
}
