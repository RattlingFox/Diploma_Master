using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master
{
    public class InitObject
    {
        public SerializableAttribute StorageNumber { get; set; }

        public int StorageSize { get; set; }

        public int FileCount { get; set; }

        public List<Files> Files { get; set; }

        public int HiveCount { get; set; }

        public int[] HivesSize { get; set; }

        public float[,] DistanceMatrix { get; set; }

        public int[,] FilesUsingMatrix { get; set; }

    }
}
