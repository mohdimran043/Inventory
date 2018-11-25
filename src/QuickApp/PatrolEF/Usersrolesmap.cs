using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Usersrolesmap
    {
        public long Userid { get; set; }
        public long Ahwalid { get; set; }
        public int Userroleid { get; set; }

        public Ahwal Ahwal { get; set; }
        public Users User { get; set; }
        public Usersroles Userrole { get; set; }
    }
}
