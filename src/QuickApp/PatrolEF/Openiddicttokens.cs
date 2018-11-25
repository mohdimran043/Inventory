using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class openiddicttokens
    {
        public string Applicationid { get; set; }
        public string Authorizationid { get; set; }
        public DateTime? Creationdate { get; set; }
        public DateTime? Expirationdate { get; set; }
        public string Concurrencytoken { get; set; }
        public string Id { get; set; }
        public string Payload { get; set; }
        public string Properties { get; set; }
        public string Referenceid { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }

        public openiddictapplications Application { get; set; }
        public openiddictauthorizations Authorization { get; set; }
    }
}
