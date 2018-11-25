using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Citygroups
    {
        public Citygroups()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
        }
        [Key]
        public long Citygroupid { get; set; }
        public long? Ahwalid { get; set; }
        public long? Sectorid { get; set; }
        public string Shortname { get; set; }
        public string Callerprefix { get; set; }
        public string Text { get; set; }
        public short Disabled { get; set; }

        public Ahwal Ahwal { get; set; }
        public Sectors Sector { get; set; }
        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
    }
}
