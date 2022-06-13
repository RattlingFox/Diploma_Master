using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Methods
{
    internal static class CriterionThree
    {
        /// <summary>
        /// Расчёт критерия К3 по формуле (2.4). Обратная пропорциональность к количеству полностью распределённых файлов в хранилище
        /// На вход запрашиваются инициализированный объект "Решение"
        /// На выходе дробное значение критерия К3
        /// </summary>
        /// <param name="solution"> Инициализированый объект "Решение" </param>
        /// <returns></returns>
        public static float CriterionThreeCalc(SolutionObject solution)
        {
            float result = 1;

            if (solution.DistributedFiles.Count != 0)
            {
                result = 1 / solution.DistributedFiles.Count;
            }

            return result;
        }
    }
}
