using System.Collections.Generic;

namespace Diploma_Master.Objects
{
    public class SolutionObject
    {
        public int[,] FileStorageMatrix { get; set; }

        public List<int> DistributedFiles { get; set; }

        public int FileSizeSum { get; set; }
    }
}