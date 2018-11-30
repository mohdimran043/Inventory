using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Handheldscheckinout
    {
        public long Handheldcheckinoutid { get; set; }
        public long Checkinoutstateid { get; set; }
        public long Handheldid { get; set; }
        public long Personid { get; set; }
        public DateTime Timestamp { get; set; }

        public Checkinoutstates Checkinoutstate { get; set; }
        public Handhelds Handheld { get; set; }
        public Persons Person { get; set; }
    }
}
