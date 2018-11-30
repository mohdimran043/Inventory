using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Operations
    {
        public Operations()
        {
            Operationlogs = new HashSet<Operationlogs>();
        }

        public int Opeartionid { get; set; }
        public string Name { get; set; }

        public ICollection<Operationlogs> Operationlogs { get; set; }
    }
}
