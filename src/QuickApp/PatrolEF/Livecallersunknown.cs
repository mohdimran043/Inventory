using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Livecallersunknown
    {
        [Key]
        public long Livecallerunknownid { get; set; }
        public string Handheldnumbername { get; set; }
        public DateTime Timestamp { get; set; }
        public short Processed { get; set; }
    }
}
