using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Incidentstates
    {
        public Incidentstates()
        {
            Incidents = new HashSet<Incidents>();
        }
        [Key]
        public int Incidentstateid { get; set; }
        public string Name { get; set; }

        public ICollection<Incidents> Incidents { get; set; }
    }
}
