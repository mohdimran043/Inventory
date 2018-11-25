using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class openiddictauthorizations
    {
        public openiddictauthorizations()
        {
            openiddicttokens = new HashSet<openiddicttokens>();
        }

        public string Applicationid { get; set; }
        public string Concurrencytoken { get; set; }
        public string Id { get; set; }
        public string Properties { get; set; }
        public string Scopes { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }

        public openiddictapplications Application { get; set; }
        public ICollection<openiddicttokens> openiddicttokens { get; set; }
    }
}
