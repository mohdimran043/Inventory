using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Operations
    {
        public Operations()
        {
            Operationlogs = new HashSet<Operationlogs>();
        }
        [Key]
        public int Opeartionid { get; set; }
        public string Name { get; set; }

        public ICollection<Operationlogs> Operationlogs { get; set; }
    }
}
