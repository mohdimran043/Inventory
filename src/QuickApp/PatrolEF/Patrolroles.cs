using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Patrolroles
    {
        public Patrolroles()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
        }

        public int Patrolroleid { get; set; }
        public string Name { get; set; }

        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
    }
}
