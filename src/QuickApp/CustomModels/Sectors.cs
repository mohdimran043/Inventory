using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModels
{
    public class Sectors
    {
        public int sectorID { get; set; }
        public int ahwalID { get; set; }
        public string shortName { get; set; }
        public string callerPrefix { get; set; }
        public int disabled { get; set; }
    }
}
