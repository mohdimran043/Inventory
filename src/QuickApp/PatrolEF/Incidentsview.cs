using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Incidentsview
    {
        [Key]
        public long Incidentid { get; set; }
        public long Userid { get; set; }
        public DateTime Timestamp { get; set; }

        public Incidents Incident { get; set; }
        public Users User { get; set; }
    }
}
