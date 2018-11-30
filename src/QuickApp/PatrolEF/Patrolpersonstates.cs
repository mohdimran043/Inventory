using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Patrolpersonstates
    {
        public Patrolpersonstates()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
            Patrolpersonstatelog = new HashSet<Patrolpersonstatelog>();
        }

        public int Patrolpersonstateid { get; set; }
        public string Name { get; set; }

        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
        public ICollection<Patrolpersonstatelog> Patrolpersonstatelog { get; set; }
    }
}
