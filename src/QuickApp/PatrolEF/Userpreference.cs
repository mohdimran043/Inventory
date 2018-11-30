using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Userpreference
    {
        public int Userpreferenceid { get; set; }
        public long? Userid { get; set; }
        public string Defaulturl { get; set; }
        public string Theme { get; set; }

        public Users User { get; set; }
    }
}
