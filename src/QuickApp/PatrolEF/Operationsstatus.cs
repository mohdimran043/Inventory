using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Operationsstatus
    {
        public Operationsstatus()
        {
            Operationlogs = new HashSet<Operationlogs>();
        }

        public int Statusid { get; set; }
        public string Name { get; set; }

        public ICollection<Operationlogs> Operationlogs { get; set; }
    }
}
