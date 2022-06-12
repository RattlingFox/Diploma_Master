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
        /// <summary>
        /// Реализация генетического алгоритма.
        /// </summary>
        /// <param name="storage"> Экземпляр "Хранилище", для которого выполняется алгоритм </param>
        /// <param name="solutionOld"> Поколение "родителей" </param>
        /// <param name="gens"> Количество генов во фрагменте хромосомы </param>
        /// <returns></returns>
        public static List<SolutionObject> GAStep(StorageObject storage, List<SolutionObject> solutionOld, int gens)
        {
            var solutionNew = new List<SolutionObject>();



            solutionNew.AddRange(solutionOld);

            return solutionNew;
        }
    }
}
