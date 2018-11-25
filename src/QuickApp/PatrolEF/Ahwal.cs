using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Ahwal
    {
        public Ahwal()
        {
            Citygroups = new HashSet<Citygroups>();
            Handhelds = new HashSet<Handhelds>();
            Patrolcars = new HashSet<Patrolcars>();
            Persons = new HashSet<Persons>();
            Sectors = new HashSet<Sectors>();
            Usersrolesmap = new HashSet<Usersrolesmap>();
        }

        public long Ahwalid { get; set; }
        public string Name { get; set; }

        public ICollection<Citygroups> Citygroups { get; set; }
        public ICollection<Handhelds> Handhelds { get; set; }
        public ICollection<Patrolcars> Patrolcars { get; set; }
        public ICollection<Persons> Persons { get; set; }
        public ICollection<Sectors> Sectors { get; set; }
        public ICollection<Usersrolesmap> Usersrolesmap { get; set; }
    }
}
