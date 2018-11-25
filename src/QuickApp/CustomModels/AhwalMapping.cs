using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModels
{
    public class AhwalMapping
    {
        public int? ahwalID { get; set; } 
        public int? sectorID { get; set; }
        public int? patrolRoleID { get; set; } 
        public int? shiftID { get; set; }
        public int personID { get; set; }
        public int? ahwalMappingID { get; set; }
        public int? cityGroupID { get; set; }
        public int? milNumber { get; set; }
        public int? rankID { get; set; }
        public string personName { get; set; }
        public string callerID { get; set; }
        public int? hasDevices { get; set; }
        public int? patrolPersonStateID { get; set; }
        public DateTime? sunRiseTimeStamp { get; set; }
        public DateTime? sunSetTimeStamp { get; set; }

        public int? sortingIndex { get; set; }
        public int? handHeldID { get; set; }

        public int? hasFixedCallerID { get; set; }

        public DateTime? lastLandTimeStamp { get; set; }
        public int? incidentID { get; set; }

        public DateTime? lastAwayTimeStamp { get; set; }

        public DateTime? lastComeBackTimeStamp { get; set; }



    }
}
