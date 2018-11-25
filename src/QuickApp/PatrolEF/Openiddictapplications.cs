using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class openiddictapplications
    {
        public openiddictapplications()
        {
            openiddictauthorizations = new HashSet<openiddictauthorizations>();
            openiddicttokens = new HashSet<openiddicttokens>();
        }

        public string Clientid { get; set; }
        public string Clientsecret { get; set; }
        public string Concurrencytoken { get; set; }
        public string Consenttype { get; set; }
        public string Displayname { get; set; }
        public string Id { get; set; }
        public string Permissions { get; set; }
        public string Postlogoutredirecturis { get; set; }
        public string Properties { get; set; }
        public string Redirecturis { get; set; }
        public string Type { get; set; }

        public ICollection<openiddictauthorizations> openiddictauthorizations { get; set; }
        public ICollection<openiddicttokens> openiddicttokens { get; set; }
    }
}
