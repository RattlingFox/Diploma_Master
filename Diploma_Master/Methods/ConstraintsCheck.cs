using Diploma_Master.Objects;
using System.Linq;

namespace Diploma_Master.Methods
{
    internal static class ConstraintsCheck
    {
        /// <summary>
        /// Проверка решения на соответствие ограничению: суммарный объём фрагментов файлов, распределённвх на узел,
        /// не превышает объём узла хранилища.
        /// На вход запрашиваются инициализированные объекты "Хранилище" и "Решение"
        /// На выходе boolean ответ о соответствии решения ограничению.
        /// </summary>
        /// <param name="storage"> Заполненный экземпляр "Хранилище" </param>
        /// <param name="solution"> Заполненный экземпляр "Решение" </param>
        /// <returns></returns>
        public static bool CheckSolution(StorageObject storage, SolutionObject solution, int gens)
        {
            var alpha = DistributedFilesSize(storage, solution, gens);

            for (int i = 1; i < storage.NodeCount; i++)
            {
                if (storage.NodesSize[i] < alpha[i])
                {
                    return false;
                }
            }            

            return true;
        }

        public static int[] DistributedFilesSize(StorageObject storage, SolutionObject solution, int gens)
        {
            var result = new int[storage.NodeCount];
            var tempResult = new int[storage.NodeCount];

            foreach (var j in solution.DistributedFiles)
            {
                for (int i = 0; i < gens; i++)
                {
                    for (int q = 0; q < storage.NodeCount; q++)
                    {
                        if (solution.FileStorageMatrix[j][i] == q)
                        {
                            var alpha = storage.Files[j]?.fileFragmentsSize[i];
                            if (alpha == null)
                            {
                                tempResult[q] += 0;
                            }
                            else
                            {
                                tempResult[q] += (int)alpha;
                            }
                            break;
                        }
                    }
                }
            }

            return tempResult;
        }
    }
}
