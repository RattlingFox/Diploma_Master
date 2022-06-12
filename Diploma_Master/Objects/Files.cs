using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master
{
    /// <summary>
    /// Класс оьбъектов типа "Файл". Содержит номер файла, его размер и массив размеров каждого фрагмента
    /// </summary>
    public class Files
    {
        public int fileNumber;

        public int fileSize;

        public int[] fileFragmentsSize;
    }
}
