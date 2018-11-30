using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Patrolpersonstatelog
    {
        public long Patrolpersonstatelogid { get; set; }
        public long Userid { get; set; }
        public long Personid { get; set; }
        public int Patrolpersonstateid { get; set; }
        public DateTime Timestamp { get; set; }

        public Patrolpersonstates Patrolpersonstate { get; set; }
        public Persons Person { get; set; }
        public Users User { get; set; }
    }
}
