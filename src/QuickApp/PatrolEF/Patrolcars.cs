using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Patrolcars
    {
        public Patrolcars()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
            Patrolcheckinout = new HashSet<Patrolcheckinout>();
        }
        [Key]
        public long Patrolid { get; set; }
        public long Ahwalid { get; set; }
        public string Platenumber { get; set; }
        public string Barcode { get; set; }
        public string Model { get; set; }
        public string Vinnumber { get; set; }
        public short Defective { get; set; }
        public short Rental { get; set; }
        public string Typecode { get; set; }

        public Ahwal Ahwal { get; set; }
        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
        public ICollection<Patrolcheckinout> Patrolcheckinout { get; set; }
    }
}
