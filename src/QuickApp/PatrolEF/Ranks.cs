using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Ranks
    {
        public Ranks()
        {
            Persons = new HashSet<Persons>();
        }

        public int Rankid { get; set; }
        public string Name { get; set; }

        public ICollection<Persons> Persons { get; set; }
    }
}
