using Diploma_Master.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Master.Research
{
    internal static class ConstraintsCheck
    {
        public static Boolean CheckSolution(InitObject storage, SolutionObject solution)
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
