using System.Collections.Generic;

namespace Diploma_Master.Objects
{
    /// <summary>
    /// Класс объектов типа "Решение". Содержит матрицу хранения фрагментов файлов на узлах хранилища,
    /// список номеров файлов, полностью распределённых по узлам, суммарный объём хранимых файлов
    /// </summary>
    public class SolutionObject
    {
        public int[,,] FileStorageMatrix { get; set; }

        public List<int> DistributedFiles { get; set; }

        public int FileSizeSum { get; set; }
    }
}