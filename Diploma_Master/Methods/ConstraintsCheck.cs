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
            int[] alpha = DistributedFilesSize(storage, solution, gens);

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
        public static int[] DistributedFilesSize(StorageObject storage, SolutionObject solution, int gens)
        {
            var tempResult = new int[storage.NodeCount];

            foreach (var j in solution.DistributedFiles)
            {
                for (int i = 0; i < gens; i++)
                {
                    for (int q = 0; q < storage.NodeCount; q++)
                    {
                        foreach (var p in solution.Files)
                        {
                            if (solution.Files[p.fileNumber].fileFragmentsStorage[i] == q)
                            {
                                int? alpha = 0;
                                
                                try
                                {
                                    alpha = solution.Files[j]?.fileFragmentsSize[i];
                                }
                                catch (Exception)
                                {
                                    tempResult[q] += 0;
                                }
                                finally
                                {
                                    if (alpha.HasValue)
                                    {
                                        tempResult[q] += (int)alpha.GetValueOrDefault(0);
                                    }
                                }
                                
                                break;
                            }
                        }
                    }
                }
            }

            return tempResult;
        }
    }
}
