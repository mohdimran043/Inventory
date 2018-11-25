using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Aspnetuserlogins
    {
        public string Loginprovider { get; set; }
        public string Providerkey { get; set; }
        public string Providerdisplayname { get; set; }
        public string Userid { get; set; }

        public Aspnetusers User { get; set; }
    }
}
