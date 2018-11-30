using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Patrolcheckinout
    {
        public long Patrolcheckinoutid { get; set; }
        public long Checkinoutstateid { get; set; }
        public long Patrolid { get; set; }
        public long Personid { get; set; }
        public DateTime Timestamp { get; set; }

        public Checkinoutstates Checkinoutstate { get; set; }
        public Patrolcars Patrol { get; set; }
        public Persons Person { get; set; }
    }
}
