using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Livecallers
    {
        public long Livecallerid { get; set; }
        public long Handheldid { get; set; }
        public DateTime Timestamp { get; set; }
        public short Processed { get; set; }

        public Handhelds Handheld { get; set; }
    }
}
