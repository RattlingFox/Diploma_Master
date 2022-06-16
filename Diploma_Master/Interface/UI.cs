using Diploma_Master.Methods;
using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Diploma_Master.System
{
    public class UI
    {
        public static StorageObject storage = new();
        public static int gens = new();
        public static List<PopulationObject> parentPopulation = new();
        public static List<PopulationObject> childPopulation = new();
        public static PopulationObject bestGeneration = new();
        public static int epochCounter = 0;

        /// <summary>
        /// Метод для запуска инициализации хранилища. Запрашивает количество фрагментов, на которые будет разделён каждый файл.
        /// </summary>
        /// <param name="gensIN"></param>
        public static void StorageInitialization(int gensIN)
        {
            gens = gensIN;
            storage = InitLoad.InitStorage();
            storage = InitLoad.InitUsingMatrix(storage);
        }

        /// <summary>
        /// Метод для вывода параметров текущего хранилища.
        /// </summary>
        public static void StoragePrint()
        {
            Console.WriteLine("Текущее хранилище:");
            Console.WriteLine($"Хранилище #{storage.StorageNumber}");
            Console.WriteLine($"Количество узлов = {storage.NodeCount}");
            Console.WriteLine($"Общий объём хранилища = {storage.StorageSize}");
            Console.WriteLine($"Количество файлов для распределения = {storage.FileCount}");
            Console.WriteLine($"Количество частей каждого файла = {gens}");
        }

        /// <summary>
        /// Метод для инициализации начального решения.
        /// </summary>
        public static void PopulationInitialization()
        {
            parentPopulation = GA.InitPopulation(storage, gens);
        }

        /// <summary>
        /// Метод для вывода параметров наилучшего из текущих решений.
        /// Выбирает поколение родителей, при отсутствии поколения детей (начальное решение).
        /// </summary>
        public static void BestObjectiveFunctionValues()
        {
            if (childPopulation.Count == 0)
            {
                bestGeneration = parentPopulation.OrderBy(x => x.ObjectiveFunctionValue).Where(y => y.ConstraintCheckResult == true).First();
            }
            else
            {
                bestGeneration = childPopulation.OrderBy(x => x.ObjectiveFunctionValue).Where(y => y.ConstraintCheckResult == true).First();
            }
            var allocatedFilesCount = storage.FileCount - bestGeneration.Solution.DistributedFiles.Count;
            var unusedSpace = storage.StorageSize - bestGeneration.Solution.FileSizeSum;
            var ObjectiveFUnctionValue = bestGeneration.ObjectiveFunctionValue;

            Console.WriteLine("Параметры наилучшего решения:");
            Console.WriteLine($"Количество нераспределённых файлов = {allocatedFilesCount}");
            Console.WriteLine($"Объём неиспользованного пространства в хранилище = {unusedSpace} Mb");
            Console.WriteLine($"Значение целевой функции = {ObjectiveFUnctionValue}");
            Console.WriteLine("");

            foreach (var j in bestGeneration.Solution.Files)
            {
                Console.WriteLine("");
                for (int i = 0; i < gens; i++)
                {
                    Console.Write($"{j.fileFragmentsStorage[i]}\t");
                }
            }

            Console.WriteLine("");

        }

        /// <summary>
        /// Метод для запуска генетического алгоритма. Выполняет 1000 эпох для текущего хранилища и начального решения.
        /// В случае, когда достигнута сходимость ранее, делоает 5 попыток запуска и останавливает выполнение алгоритма.
        /// </summary>
        public static void GARealization()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (childPopulation.Count != 0)
            {
                parentPopulation = childPopulation;
            }

            for (; epochCounter < 100; epochCounter++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (childPopulation.Count <= 1)
                    {
                        childPopulation = GA.GAStep(storage, parentPopulation, gens);
                    }
                }

            }

            if (epochCounter == 1000)
            {
                Console.WriteLine($"Достигнута {epochCounter}-ая эпоха");
            }

            else
            {
                Console.WriteLine($"Достигнута сходимость за {epochCounter} эпох");
            }

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine();
            Console.WriteLine($"Время работы алгоритма {ts}");

        }

    }
}
