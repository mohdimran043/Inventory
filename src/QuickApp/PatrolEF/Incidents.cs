using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Incidents
    {
        public Incidents()
        {
            Ahwalmapping = new HashSet<Ahwalmapping>();
            Incidentscomments = new HashSet<Incidentscomments>();
            Incidentsview = new HashSet<Incidentsview>();
        }
        [Key]
        public long Incidentid { get; set; }
        public int Incidenttypeid { get; set; }
        public int Incidentstateid { get; set; }
        public string Place { get; set; }
        public int Incidentsourceid { get; set; }
        public string Incidentsourceextrainfo1 { get; set; }
        public string Incidentsourceextrainfo2 { get; set; }
        public string Incidentsourceextrainfo3 { get; set; }
        public long Userid { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime Lastupdate { get; set; }

        public Incidentsources Incidentsource { get; set; }
        public Incidentstates Incidentstate { get; set; }
        public Incidentstypes Incidenttype { get; set; }
        public Users User { get; set; }
        public ICollection<Ahwalmapping> Ahwalmapping { get; set; }
        public ICollection<Incidentscomments> Incidentscomments { get; set; }
        public ICollection<Incidentsview> Incidentsview { get; set; }
    }
}
