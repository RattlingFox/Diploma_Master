using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diploma_Master.Methods
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
            var rnd = new Random();

            var solution = new SolutionObject()
            {
                Files = new List<Files>(),
                DistributedFiles = new List<int>(),
                FileSizeSum = 0
            };

            // Инициализация объектов "Файл" в объекте "Хранилище"
            solution = InitFiles(storage, solution, gens);

            // Перебор файлов в хранилище для формировани списка полностью распределённых файлов
            RecalcDistributedFiles(gens, solution);

            // Перебор файлов в хранилище для подсчёта суммарного объёма
            RecalcFileSizeSum(storage, solution);

            return solution;
        }

        /// <summary>
        /// Инициализация объектов "Файл" в объекте "Хранилище".
        /// На вход запрашиваем инициализированный экземпляр "Хранилище" и количество фрагментов, на которое делим каждый файл.
        /// На выходе получим дополненный экземпляр "Хранилище" с заполненным списком файлов, размеры файлов, размеры фрагментов файлов
        /// </summary>
        /// <param name="storage"> Инициализированное пустое хранилище </param>
        /// <param name="gens"> Количество фрагментов, на которое делим каждый файл </param>
        /// <returns></returns>
        public static SolutionObject InitFiles(StorageObject storage, SolutionObject solution, int gens)
        {
            var rnd = new Random();

            for (int h = 0; h < storage.FileCount; h++)
            {
                solution.Files.Add(new Files());

                solution.Files[h].fileNumber = h;
                //alpha.fileSize = rnd.Next(1, 128) * gens;
                solution.Files[h].fileSize = 256 * gens;
                solution.Files[h].fileFragmentsCount = gens;
                solution.Files[h].fileFragmentsSize = new List<int>();
                solution.Files[h].fileFragmentsStorage = new List<int>();

                for (int i = 0; i < gens; i++)
                {

                    var rndNum = rnd.Next(0, storage.NodeCount - 1);

                    //solution.Files[h].fileFragmentsSize.Add(solution.Files[h].fileSize / gens);
                    solution.Files[h].fileFragmentsSize.Add(256);

                    while (solution.Files.Any(x => x.fileFragmentsStorage.Where(y => y == rndNum).Count() >= 5))
                    {
                        rndNum = 0;
                    }

                    solution.Files[h].fileFragmentsStorage.Add(rndNum);
                }                
            }          

            return solution;
        }

    /// <summary>
    /// Метод для пересчёта параметров нового решения. Пересчитывает список полностью распределённых файлов по узлам.
    /// На вход метод запрашивает количество частеЙ, на который делиться каждый файл и экземпляр "решения" для пересчёта с новой матрицей хранения.
    /// На выходе метод отдаёт экземпляр "Решения" с новыми значениями параметров.
    /// </summary>
    /// <param name="solution"> Экземпляр "Решение" </param>
    /// <param name="gens"> Количество частей, на которые делится каждый файл </param>
    /// <returns></returns>
    public static SolutionObject RecalcDistributedFiles(int gens, SolutionObject solution)
        {
            solution.DistributedFiles = new List<int>();

            int iter = 0;
            foreach (var j in solution.Files)
            {
                int alpha = 0;
                foreach (var i in j.fileFragmentsStorage)
                {
                    var check = true;
                    if (i == 0)
                    {
                        check = false;
                        break;
                    }
                    if (check == true && alpha == gens - 1)
                    {
                        solution.DistributedFiles.Add(iter);
                    }
                    alpha++;
                }

                iter++;
            }

            return solution;
        }

        /// <summary>
        /// Метод для пересчёта параметров нового решения. Пересчитывает общий объём полностью распределённых файлов по узлам.
        /// На вход метод запрашивает экземпляр "Хранилище" и экземпляр "решения" для пересчёта с новой матрицей хранения.
        /// На выходе метод отдаёт экземпляр "Решения" с новыми значениями параметров.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище" </param>
        /// <param name="solution"> Экземпляр "Решение" </param>
        /// <returns></returns>
        public static SolutionObject RecalcFileSizeSum(StorageObject storage, SolutionObject solution)
        {
            var iter = 0;
            // Перебор файлов в хранилище для подсчёта суммарного объёма
            foreach (Files j in solution.Files)
            {
                if (solution.DistributedFiles.Contains(j.fileNumber))
                {
                    solution.FileSizeSum += j.fileSize;
                };

                iter++;
            }

            return solution;
        }        

        /// <summary>
        /// Метод для получения матрицы хранения вида int[q,j,i] для расчёта критериев К1 и К2.
        /// На вход метод запрашивает экземпляр текущего "Хранилища", экземпляр текущего "Решения" и количество фрагментов каждого файла.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище" </param>
        /// <param name="solution"> Экземпляр "Решение" </param>
        /// <param name="gens"> Количество фрагментов каждого файла </param>
        /// <returns></returns>
        public static int[,,] StorageMatrixToArray(StorageObject storage, SolutionObject solution, int gens)
        {
            var result = new int[storage.NodeCount, storage.FileCount, gens];

            foreach (var j in solution.Files)
            {
                for (int q = 0; q < storage.NodeCount; q++)
                {
                    for (int i = 0; i < j.fileFragmentsCount; i++)
                    {
                        if (j.fileFragmentsStorage[i] == q)
                        {
                            result[q, j.fileNumber, i] = 1;
                        }
                        else
                        {
                            result[q, j.fileNumber, i] = 0;
                        }
                    }
                }
            }

            return result;
        }
    }
}