using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Usersroles
    {
        public Usersroles()
        {
            Usersrolesmap = new HashSet<Usersrolesmap>();
        }

        public int Userroleid { get; set; }
        public string Name { get; set; }

        public ICollection<Usersrolesmap> Usersrolesmap { get; set; }
    }
}
