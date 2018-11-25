using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Incidentstypes
    {
        public Incidentstypes()
        {
            Incidents = new HashSet<Incidents>();
        }
        [Key]
        public int Incidenttypeid { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }

        public ICollection<Incidents> Incidents { get; set; }
    }
}
