using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class OpenIddictAuthorizations
    {
        public OpenIddictAuthorizations()
        {
            OpenIddictTokens = new HashSet<OpenIddictTokens>();
        }

        public string ApplicationId { get; set; }
        public string ConcurrencyToken { get; set; }
        public string Id { get; set; }
        public string Properties { get; set; }
        public string Scopes { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }

        public OpenIddictApplications Application { get; set; }
        public ICollection<OpenIddictTokens> OpenIddictTokens { get; set; }
    }
}
