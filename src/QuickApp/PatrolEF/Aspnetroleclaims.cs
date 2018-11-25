using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Aspnetroleclaims
    {
        public int Id { get; set; }
        public string Roleid { get; set; }
        public string Claimtype { get; set; }
        public string Claimvalue { get; set; }

        public Aspnetroles Role { get; set; }
    }
}
