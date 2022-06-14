using Diploma_Master.Objects;

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
            var answer = true;
            var alpha = DistributedFilesSize(storage, solution, gens);

            for (int i = 0; i < storage.HiveCount; i++)
            {
                if (storage.HivesSize[i] < alpha[i])
                {
                    answer = false;
                }
            }            

            return answer;
        }

        public static int[] DistributedFilesSize(StorageObject storage, SolutionObject solution, int gens)
        {
            var result = new int[storage.HiveCount];
            int iter = 0;

            foreach (var j in solution.FileStorageMatrix)
            {
                for (int i = 0; i < gens; i++)
                {
                    for (int q = 0; q < storage.HiveCount; q++)
                    {
                        if (j[i] == q)
                        {
                            result[q] += storage.Files[iter].fileSize/gens;
                        }
                    }
                }

                iter++;
            }

            return result;
        }
    }
}
