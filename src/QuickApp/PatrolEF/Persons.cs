using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Persons
    {
        public Persons()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
            Handheldscheckinout = new HashSet<Handheldscheckinout>();
            Patrolcheckinout = new HashSet<Patrolcheckinout>();
            Patrolpersonstatelog = new HashSet<Patrolpersonstatelog>();
        }
        [Key]
        public long Personid { get; set; }
        public long Ahwalid { get; set; }
        public long Milnumber { get; set; }
        public int Rankid { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Fixedcallerid { get; set; }

        public Ahwal Ahwal { get; set; }
        public Ranks Rank { get; set; }
        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
        public ICollection<Handheldscheckinout> Handheldscheckinout { get; set; }
        public ICollection<Patrolcheckinout> Patrolcheckinout { get; set; }
        public ICollection<Patrolpersonstatelog> Patrolpersonstatelog { get; set; }
    }
}
