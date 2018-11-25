using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Sectors
    {
        public Sectors()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
            Citygroups = new HashSet<Citygroups>();
        }

        public long Sectorid { get; set; }
        public long? Ahwalid { get; set; }
        public string Shortname { get; set; }
        public string Callerprefix { get; set; }
        public short Disabled { get; set; }

        public Ahwal Ahwal { get; set; }
        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
        public ICollection<Citygroups> Citygroups { get; set; }
    }
}
