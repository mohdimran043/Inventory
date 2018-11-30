using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Incidentscomments
    {
        public long Incidentcommentid { get; set; }
        public long Incidentid { get; set; }
        public string Text { get; set; }
        public long Userid { get; set; }
        public DateTime Timestamp { get; set; }

        public Incidents Incident { get; set; }
        public Users User { get; set; }
    }
}
