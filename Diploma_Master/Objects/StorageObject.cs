using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master
{
    /// <summary>
    /// Класс объектов "Хранилище". Содержит номер экземпляра, количество узлов, массив размеров каждого узла, общий объём хранилища,
    /// количество распределяемых файлов, список объектов типа "Файл", матрицу расстояний между узлами, матрицу использования файлов узлами
    /// </summary>
    public class StorageObject
    {
        public SerializableAttribute StorageNumber { get; set; }

        public int HiveCount { get; set; }

        public int[] HivesSize { get; set; }

        public int StorageSize { get; set; }

        public int FileCount { get; set; }

        public List<Files> Files { get; set; }

        public float[,] DistanceMatrix { get; set; }

        public int[,] FilesUsingMatrix { get; set; }

    }
}
