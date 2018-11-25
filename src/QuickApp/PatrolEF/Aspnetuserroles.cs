using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Aspnetuserroles
    {
        public string Userid { get; set; }
        public string Roleid { get; set; }

        public Aspnetroles Role { get; set; }
        public Aspnetusers User { get; set; }
    }
}
