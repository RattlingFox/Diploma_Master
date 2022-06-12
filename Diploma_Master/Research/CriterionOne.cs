using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Research
{
    internal static class CriterionOne
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage"> Инициализированный обект storage (хранилище) </param>
        /// <param name="solution"> Инициализированый объект solution (решение) </param>
        /// <returns></returns>
        public static float CriterionOneCalc(InitObject storage, SolutionObject solution)
        {
            float result = 0;

            for(int h = 0; h <= storage.HiveCount; h++)
            {
                int filesSize = 0;
                foreach (Files j in storage.Files)
                {
                    if (solution.DistributedFiles.Contains(j.fileNumber))
                    {
                        for (int i = 0; i <= j.fileFragmentsSize.Length; i++)
                        {
                            filesSize += i * solution.FileStorageMatrix[h, j.fileNumber,i];
                        }
                    }                    
                }
                result += storage.HivesSize[h] - filesSize;
            }

            return result;
        }
    }
}
