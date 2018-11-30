using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Incidentstates
    {
        public Incidentstates()
        {
            Incidents = new HashSet<Incidents>();
        }

        public int Incidentstateid { get; set; }
        public string Name { get; set; }

        public ICollection<Incidents> Incidents { get; set; }
    }
}
