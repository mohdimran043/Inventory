using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOI.Patrol
{
    public partial class Ahwalmapping
    {
        [Key]
        public long Ahwalmappingid { get; set; }
        public long Ahwalid { get; set; }
        public int Shiftid { get; set; }
        public long Sectorid { get; set; }
        public int Patrolroleid { get; set; }
        public long Citygroupid { get; set; }
        public long Personid { get; set; }
        public short Hasfixedcallerid { get; set; }
        public string Callerid { get; set; }
        public short Hasdevices { get; set; }
        public long? Handheldid { get; set; }
        public long? Patrolid { get; set; }
        public int? Patrolpersonstateid { get; set; }
        public DateTime? Laststatechangetimestamp { get; set; }
        public DateTime? Sunrisetimestamp { get; set; }
        public DateTime? Sunsettimestamp { get; set; }
        public DateTime? Lastlandtimestamp { get; set; }
        public DateTime? Lastseatimestamp { get; set; }
        public DateTime? Lastawaytimestamp { get; set; }
        public DateTime? Lastcomebacktimestamp { get; set; }
        public long? Incidentid { get; set; }
        public long? Associtatepersonid { get; set; }
        public long Sortingindex { get; set; }

        public Citygroups Citygroup { get; set; }
        public Handhelds Handheld { get; set; }
        public Incidents Incident { get; set; }
        public Patrolcars Patrol { get; set; }
        public Patrolpersonstates Patrolpersonstate { get; set; }
        public Patrolroles Patrolrole { get; set; }
        public Persons Person { get; set; }
        public Sectors Sector { get; set; }
        public Shifts Shift { get; set; }
    }
}
