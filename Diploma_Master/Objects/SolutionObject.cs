using System.Collections.Generic;

namespace Diploma_Master.Objects
{
    /// <summary>
    /// Класс объектов типа "Решение". Содержит матрицу хранения фрагментов файлов на узлах хранилища,
    /// список номеров файлов, полностью распределённых по узлам, суммарный объём хранимых файлов
    /// </summary>
    public class SolutionObject
    {
        public List<Files> Files { get; set; } = new List<Files>();

        public List<int> DistributedFiles { get; set; } = new List<int>();

        public int FileSizeSum { get; set; }
    }
}