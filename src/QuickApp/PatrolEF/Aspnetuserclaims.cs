using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Aspnetuserclaims
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string Claimtype { get; set; }
        public string Claimvalue { get; set; }

        public Aspnetusers User { get; set; }
    }
}
