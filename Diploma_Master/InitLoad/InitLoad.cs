using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master
{
    public class InitLoad
    {
        private static readonly Random rnd = new();

        // Инициализация экземпляра storage (хранилище)
        // На выходе получим экземпляр storage (хранилище),
        // для которого заполнены параметры: номер, количество узлов, объёмы узлов,
        // объём всего хранилища, количество файлов для распределения в хранилище
        public static InitObject InitStorage()                          
        {
            var storage = new InitObject
            {
                StorageNumber = new SerializableAttribute(),            // Присваиваем номер экземпляру, как элемент последовательности

                HiveCount = rnd.Next(100, 500)                          // Задаём количество узлов в хранилище
            };                                                          // Создаём пустой экземляр класса storage (хранилище)

            int sizeAll = 0;                                            // Создаём переменную для объёма хранилища

            for (int j = 0; j <= storage.HiveCount; j++)
            {
                storage.HivesSize[j] = rnd.Next(20, 100 * (10^6));      // Генерируем объём текущего узла
                sizeAll += storage.HivesSize[j];                               // Суммируем объёмы узлов
            }

            storage.StorageSize = sizeAll;                              // Заполняем параметр объём хранилища

            storage.FileCount = rnd.Next(1000, 10000);                  // Заполняем параметр количество файлов, распределяемых по узлам

            for (int i = 0; i <= storage.HiveCount; i++)
            {
                for (int g = 0; g <= storage.HiveCount; g++)
                {
                    if (i == g)
                    {
                        storage.DistanceMatrix[i, g] = 0;
                    }
                    else
                    {
                        storage.DistanceMatrix[i, g] = 
                            rnd.Next(0, 10)/10;                         // Генерируем матрицу расстояний между узлами
                    }
                }
            }

            return storage;
        }

        // Инициализация файлов в объекте storage (хранилище)
        // На вход запрашиваем инициализированный экземпляр storage (хранилище) и количество фрагментов,
        // на которое делим каждый файл
        // На выходе получим дополненный экземпляр storage (хранилище) с заполненным списком файлов,
        // размеры файлов, размеры фрагментов файлов
        public static InitObject InitFiles(InitObject storage, 
            int gens)                                                  
        {
            var fileList = new List<Files>();                           // Создаём пустой список объектов файл

            for (int h = 0; h <= storage.FileCount; h++)
            {
                var fileSize = rnd.Next(1 * (10 ^ 2), 2 * (10 ^ 9));    // Генерируем размер h-ого файла
                int fragmentSize = fileSize / gens;                     // Генерируем размер фрагментов h-ого файла
                var fileFragmentsSize = new int[gens];                  // Создаём пустой массив фрагментов h-ого файла
                for (int j = 0; j < gens; j++)
                {
                    fileFragmentsSize[j] = fragmentSize;                // Генерируем массив размеров фрагментов h-ого файла
                }

                fileList.Add(new Files
                {
                    fileNumber = h,
                    fileSize = fileSize,
                    fileFragmentsSize = fileFragmentsSize
                });                                                     // Заполняем список файлов в переменной

                storage.Files = fileList.ToList();                      // Заполняем список файлов в объекте storage (хранилище)
            }

                return storage;
        }

        // Иницилизация матрицы использования файлов узлами хранилища
        // На вход запрашиваем инициализированный экземпляр storage (хранилище)
        // На выходе получим дополненный экземпляр storage (хранилище) с заполненной матрицей использования файлов
        // Марица заполняется случайным образом с вероятностью 0,12 на использование и 0,88 на неиспользование
        public static InitObject InitUsingMatrix(InitObject storage)         
        {
            for (int i = 0; i <= storage.FileCount; i++)
            {
                for (int j = 0; j <= storage.HiveCount; j++)
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
