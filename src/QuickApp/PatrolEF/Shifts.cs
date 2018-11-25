using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Shifts
    {
        public Shifts()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
        }

        public int Shiftid { get; set; }
        public string Name { get; set; }
        public int Startinghour { get; set; }
        public int Numberofhours { get; set; }

        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
    }
}
