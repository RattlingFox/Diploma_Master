using System;
using System.Collections.Generic;
using System.Linq;

namespace Diploma_Master.Methods
{
    public class InitLoad
    {
        private static readonly Random rnd = new();

        /// <summary>
        /// Инициализация экземпляра "Хранилище".
        /// На выходе получим экземпляр "Хранилище", для которого заполнены параметры: 
        /// номер, количество узлов, объёмы узлов, объём всего хранилища, количество файлов для распределения в хранилище
        /// </summary>
        /// <returns></returns>
        public static StorageObject InitStorage()
        {
            var storage = new StorageObject
            {
                // Присваиваем номер экземпляру, как элемент последовательности
                StorageNumber = 1,

                // Задаём количество узлов в хранилище
                //HiveCount = rnd.Next(20, 100)
                HiveCount = 50
            };

            int sizeAll = 0;

            storage.HivesSize = new int[storage.HiveCount];
            for (int j = 0; j < storage.HiveCount; j++)
            {
                // Генерируем объём текущего узла в Мб
                //storage.HivesSize[j] = rnd.Next(4048, 10240);
                storage.HivesSize[j] = 4048;
                // Суммируем объёмы узлов
                sizeAll += storage.HivesSize[j];
            }

            storage.StorageSize = sizeAll;

            // Заполняем параметр количество файлов, распределяемых по узлам, умножаем на 2 для получения чётного
            //storage.FileCount = rnd.Next(25, 30)*2;
            storage.FileCount = 150;

            storage.DistanceMatrix = new float[storage.HiveCount, storage.HiveCount];
            for (int i = 0; i < storage.HiveCount; i++)
            {
                for (int g = 0; g < storage.HiveCount; g++)
                {
                    if (i == g)
                    {
                        storage.DistanceMatrix[i, g] = 0;
                    }
                    else
                    {
                        // Генерируем матрицу расстояний между узлами
                        // Возможные значения от 0.1 до 1.0 без указания размерности
                        storage.DistanceMatrix[i, g] = rnd.Next(0, 10) / 10;
                    }
                }
            }

            return storage;
        }

        /// <summary>
        /// Инициализация файлов в объекте "Хранилище".
        /// На вход запрашиваем инициализированный экземпляр "Хранилище" и количество фрагментов, на которое делим каждый файл.
        /// На выходе получим дополненный экземпляр "Хранилище" с заполненным списком файлов, размеры файлов, размеры фрагментов файлов
        /// </summary>
        /// <param name="storage"> Инициализированное пустое хранилище </param>
        /// <param name="gens"> Количество фрагментов, на которое делим каждый файл </param>
        /// <returns></returns>
        public static StorageObject InitFiles(StorageObject storage,
            int gens)
        {
            // Создаём пустой список объектов файл
            var fileList = new List<Files>();

            for (int h = 0; h <= storage.FileCount; h++)
            {
                // Генерируем размер h-ого файла в Мб
                //var fileSize = rnd.Next(1, 128)*gens;
                var fileSize = 256 * gens;
                // Генерируем размер фрагментов h-ого файла
                int fragmentSize = fileSize / gens;
                // Создаём пустой массив фрагментов h-ого файла
                var fileFragmentsSize = new int[gens];
                for (int j = 0; j < gens; j++)
                {
                    // Генерируем массив размеров фрагментов h-ого файла
                    fileFragmentsSize[j] = fragmentSize;
                }

                // Заполняем список файлов в переменной
                fileList.Add(new Files
                {
                    fileNumber = h,
                    fileSize = fileSize,
                    fileFragmentsSize = fileFragmentsSize
                });

                // Заполняем список файлов в объекте хранилище
                storage.Files = fileList.ToList();
            }

            return storage;
        }

        /// <summary>
        /// Иницилизация матрицы использования файлов узлами хранилища.
        /// На вход запрашиваем инициализированный экземпляр "Хранилища".
        /// На выходе получим дополненный экземпляр "Хранилище" с заполненной матрицей использования файлов.
        /// Матрица заполняется случайным образом с вероятностью 0,12 на использование и 0,88 на неиспользование
        /// </summary>
        /// <param name="storage"> Инициализированное хранилище с файлами </param>
        /// <returns></returns>
        public static StorageObject InitUsingMatrix(StorageObject storage)
        {
            storage.FilesUsingMatrix = new int[storage.FileCount, storage.HiveCount];
            for (int i = 0; i < storage.FileCount; i++)
            {
                for (int j = 0; j < storage.HiveCount; j++)
                {
                    int g = rnd.Next(0, 100);
                    if (g <= 5 || g >= 95)
                    {
                        storage.FilesUsingMatrix[i, j] = 1;
                    }
                    else
                    {
                        storage.FilesUsingMatrix[i, j] = 0;
                    }
                }
            }

            return storage;
        }

    }
}
