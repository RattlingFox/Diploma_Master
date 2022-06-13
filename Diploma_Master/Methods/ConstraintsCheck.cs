using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Methods
{
    internal static class ConstraintsCheck
    {
        /// <summary>
        /// Проверка решения на соответствие ограничению: суммарный объём фрагментов файлов, распределённвх на узел,
        /// не превышает объём узла хранилища.
        /// На вход запрашиваются инициализированные объекты "Хранилище" и "Решение"
        /// На выходе boolean ответ о соответствии решения ограничению.
        /// </summary>
        /// <param name="storage"> Заполненный экземпляр "Хранилище" </param>
        /// <param name="solution"> Заполненный экземпляр "Решение" </param>
        /// <returns></returns>
        public static bool CheckSolution(StorageObject storage, SolutionObject solution)
        {
            var answer = false;

            if (storage.StorageSize >= solution.FileSizeSum)
            {
                answer = true;
            }

            return answer;
        }
    }
}
