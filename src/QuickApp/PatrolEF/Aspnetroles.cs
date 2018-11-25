using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Aspnetroles
    {
        public Aspnetroles()
        {
            Aspnetroleclaims = new HashSet<Aspnetroleclaims>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Normalizedname { get; set; }
        public string Concurrencystamp { get; set; }
        public string Description { get; set; }
        public string Createdby { get; set; }
        public string Updatedby { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }

        public ICollection<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        public ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
    }
}
