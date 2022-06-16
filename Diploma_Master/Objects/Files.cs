using System.Collections.Generic;

namespace Diploma_Master
{
    /// <summary>
    /// Класс оьбъектов типа "Файл". Содержит номер файла, его размер и массив размеров каждого фрагмента
    /// </summary>
    public class Files
    {
        public int fileNumber { get; set; }

        public int fileSize { get; set; }

        public int fileFragmentsCount { get; set; }

        public List<int> fileFragmentsSize { get; set; } = new List<int>();

        public List<int> fileFragmentsStorage { get; set; } = new List<int>();
    }
}
