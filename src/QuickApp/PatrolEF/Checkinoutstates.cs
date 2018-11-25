using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Checkinoutstates
    {
        public Checkinoutstates()
        {
            Handheldscheckinout = new HashSet<Handheldscheckinout>();
            Patrolcheckinout = new HashSet<Patrolcheckinout>();
        }
        [Key]
        public long Checkinoutstateid { get; set; }
        public string Name { get; set; }

        public ICollection<Handheldscheckinout> Handheldscheckinout { get; set; }
        public ICollection<Patrolcheckinout> Patrolcheckinout { get; set; }
    }
}
