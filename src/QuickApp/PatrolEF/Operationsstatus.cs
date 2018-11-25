using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Operationsstatus
    {
        public Operationsstatus()
        {
            Operationlogs = new HashSet<Operationlogs>();
        }
        [Key]
        public int Statusid { get; set; }
        public string Name { get; set; }

        public ICollection<Operationlogs> Operationlogs { get; set; }
    }
}
