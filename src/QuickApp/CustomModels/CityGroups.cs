using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModels
{
    public class CityGroups
    {

        public int cityGroupID { get; set; }
        public int ahwalID { get; set; }
        public int sectorID { get; set; }
        public string shortName { get; set; }
        public string callerPrefix { get; set; }
        public string text { get; set; }
        public int disabled { get; set; }
    }
}
