using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Incidentsources
    {
        public Incidentsources()
        {
            Incidents = new HashSet<Incidents>();
        }
        [Key]
        public int Incidentsourceid { get; set; }
        public string Name { get; set; }
        public int Mainextrainfonumber { get; set; }
        public string Extrainfo1 { get; set; }
        public string Extrainfo2 { get; set; }
        public string Extrainfo3 { get; set; }
        public short Requiresextrainfo1 { get; set; }
        public short Requiresextrainfo2 { get; set; }
        public short Requiresextrainfo3 { get; set; }

        public ICollection<Incidents> Incidents { get; set; }
    }
}
