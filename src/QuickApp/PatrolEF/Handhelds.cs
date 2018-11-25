using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Handhelds
    {
        public Handhelds()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
            Handheldscheckinout = new HashSet<Handheldscheckinout>();
            Livecallers = new HashSet<Livecallers>();
        }
        [Key]
        public long Handheldid { get; set; }
        public long Ahwalid { get; set; }
        public string Serial { get; set; }
        public string Barcode { get; set; }
        public short Defective { get; set; }

        public Ahwal Ahwal { get; set; }
        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
        public ICollection<Handheldscheckinout> Handheldscheckinout { get; set; }
        public ICollection<Livecallers> Livecallers { get; set; }
    }
}
