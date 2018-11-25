using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Users
    {
        public Users()
        {
            Incidents = new HashSet<Incidents>();
            Incidentscomments = new HashSet<Incidentscomments>();
            Incidentsview = new HashSet<Incidentsview>();
            Operationlogs = new HashSet<Operationlogs>();
            Patrolpersonstatelog = new HashSet<Patrolpersonstatelog>();
            Usersrolesmap = new HashSet<Usersrolesmap>();
        }

        public long Userid { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int Failedlogins { get; set; }
        public DateTime? Lastsuccesslogin { get; set; }
        public DateTime? Lastfailedlogin { get; set; }
        public string Lastipaddress { get; set; }
        public short Accountlocked { get; set; }
        public string LayoutAhwalmapping { get; set; }
        public string LayoutGroupsAhawalmapping { get; set; }
        public string LayoutOpslive { get; set; }
        public string LayoutGroupsOpslivegrid { get; set; }

        public ICollection<Incidents> Incidents { get; set; }
        public ICollection<Incidentscomments> Incidentscomments { get; set; }
        public ICollection<Incidentsview> Incidentsview { get; set; }
        public ICollection<Operationlogs> Operationlogs { get; set; }
        public ICollection<Patrolpersonstatelog> Patrolpersonstatelog { get; set; }
        public ICollection<Usersrolesmap> Usersrolesmap { get; set; }
    }
}
