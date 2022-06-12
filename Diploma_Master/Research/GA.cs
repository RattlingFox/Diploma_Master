using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Diploma_Master.Objects;

namespace Diploma_Master
{
    public class GA
    {
        public static List<SolutionObject> GAStep(InitObject storage, List<SolutionObject> solutionOld, int gens)
        {
            var solutionNew = new List<SolutionObject>();



            solutionNew.AddRange(solutionOld);

            return solutionNew;
        }
    }
}
