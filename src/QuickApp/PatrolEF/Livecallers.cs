using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Livecallers
    {
        [Key]
        public long Livecallerid { get; set; }
        public long Handheldid { get; set; }
        public DateTime Timestamp { get; set; }
        public short Processed { get; set; }

        public Handhelds Handheld { get; set; }
    }
}
