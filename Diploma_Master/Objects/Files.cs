using System.Collections.Generic;

namespace Diploma_Master
{
    /// <summary>
    /// Класс оьбъектов типа "Файл". Содержит номер файла, его размер и массив размеров каждого фрагмента
    /// </summary>
    public class Files
    {
        public int fileNumber;

        public int fileSize;

        public int fileFragmentsCount;

        public List<int> fileFragmentsSize;

        public List<int> fileFragmentsStorage;
    }
}
