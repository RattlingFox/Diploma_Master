using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
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
            List<int> alpha = DistributedFilesSize(storage, solution, gens);

            for (int i = 1; i < storage.NodeCount; i++)
            {
                if (storage.NodesSize[i] < alpha[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Метод для подсчёта сколько места занимают фрагменты файлов на каждом узле
        /// На вход запрашиваются инициализированные объекты "Хранилище" и "Решение", и количество фрагментов каждого файла
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище" </param>
        /// <param name="solution"> Экземпляр "Решение" </param>
        /// <param name="gens"> Количество частей каждого файла </param>
        /// <returns></returns>
        public static List<int> DistributedFilesSize(StorageObject storage, SolutionObject solution, int gens)
        {
            var tempResult = new List<int>();
            for (int i = 0; i < storage.NodeCount; i++)
            {
                tempResult.Add(0);
            }

            int a = 0;

            foreach (var j in solution.DistributedFiles)
            {
                for (int i = 0; i < gens; i++)
                {
                    for (int q = 0; q < storage.NodeCount; q++)
                    {
                        if (solution.Files[j.fileNumber].fileFragmentsStorage[i] == q)
                        {
                            int? alpha = 0;

                            alpha = j?.fileFragmentsSize[i];

                            if (alpha == null)
                            {
                                tempResult[q] += 0;
                            }

                            tempResult[q] += alpha ?? 0;

                            a += alpha ?? 0;

                            break;
                        }
                    }
                }
            }

            return tempResult;
        }
    }
}
