using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class OpenIddictTokens
    {
        public string ApplicationId { get; set; }
        public string AuthorizationId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ConcurrencyToken { get; set; }
        public string Id { get; set; }
        public string Payload { get; set; }
        public string Properties { get; set; }
        public string ReferenceId { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }

        public OpenIddictApplications Application { get; set; }
        public OpenIddictAuthorizations Authorization { get; set; }
    }
}
