using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Core;
using MOI.Patrol.DataAccessLayer;

namespace MOI.Patrol.Controllers
{
    [Route("api/[controller]")]
    public class OperationsController : Controller
    {

        private Handler_Person _person = new Handler_Person();
        private Handler_User _user = new Handler_User();
        private Handler_PatrolCars _patrol = new Handler_PatrolCars();
        private Handler_HandHelds _handheld = new Handler_HandHelds();
        private Handler_AhwalMapping _ahwalmapping = new Handler_AhwalMapping();
        private patrolsContext _context = new patrolsContext();

        private String constr = "server=localhost;Port=5432;User Id=postgres;password=12345;Database=Patrols";
        private DataAccess DAL = new DataAccess();
        private Handler_Operations _oper = new Handler_Operations();
        private Handler_Incidents _inc = new Handler_Incidents();
        private Handler_IncidentTypes _incTypes = new Handler_IncidentTypes();
        [HttpPost("operationslist")]
        public ActionResult PostOperationsList()
        {
           
            string Qry = "SELECT         Incidents.IncidentID, Incidents.IncidentStateID,Users.Name as UserName, Incidents.Place, Incidents.IncidentSourceExtraInfo1 as ExtraInfo1, Incidents.IncidentSourceExtraInfo2 as ExtraInfo2, Incidents.IncidentSourceExtraInfo3  as ExtraInfo3, Incidents.TimeStamp, Incidents.LastUpdate, Incidents.IncidentSourceID, IncidentsTypes.Name AS IncidentsTypeName, IncidentSources.Name AS IncidentSourceName, IncidentSources.ExtraInfo1 as IncidentSourceExtraInfo1, IncidentSources.ExtraInfo2 as IncidentSourceExtraInfo2, IncidentSources.ExtraInfo3 as IncidentSourceExtraInfo3 FROM   Incidents INNER JOIN IncidentSources ON Incidents.IncidentSourceID = IncidentSources.IncidentSourceID INNER JOIN IncidentStates ON Incidents.IncidentStateID = IncidentStates.IncidentStateID INNER JOIN IncidentsTypes ON Incidents.IncidentTypeID = IncidentsTypes.IncidentTypeID  INNER JOIN Users ON Incidents.UserID = Users.UserID where Incidents.IncidentStateID!=30 Order by TimeStamp desc LIMIT 50 OFFSET 1";

            return Ok(DAL.PostGre_GetDataTable(Qry));
        }

        [HttpPost("incidentview")]
        public ActionResult PostIncidentView([FromBody]JObject RqHdr)
        {
            var incidentID = Convert.ToDouble(RqHdr["incidentid"]);
            Users user = new Users();
            user.Userid = Convert.ToInt32(RqHdr["userid"]);

            //var isInIncidentview = _context.Incidentsview.FirstOrDefault<Incidentsview>(a => a.Userid == user.Userid && a.Incidentid == incidentID);
            var isInIncidentview = _context.Incidentsview.FirstOrDefault<Incidentsview>(a => a.Incidentid == incidentID);
            return Ok(isInIncidentview);
        }

        [HttpPost("incidentsources")]
        public ActionResult PostIncidentSourcesList()
        {

            string Qry = "SELECT        Incidents.IncidentID, Incidents.IncidentStateID,Users.Name as UserName, Incidents.Place, Incidents.IncidentSourceExtraInfo1 as ExtraInfo1, Incidents.IncidentSourceExtraInfo2 as ExtraInfo2, Incidents.IncidentSourceExtraInfo3  as ExtraInfo3, Incidents.TimeStamp, Incidents.LastUpdate, Incidents.IncidentSourceID, IncidentsTypes.Name AS IncidentsTypeName, IncidentSources.Name AS IncidentSourceName, IncidentSources.ExtraInfo1 as IncidentSourceExtraInfo1, IncidentSources.ExtraInfo2 as IncidentSourceExtraInfo2, IncidentSources.ExtraInfo3 as IncidentSourceExtraInfo3 FROM   Incidents INNER JOIN IncidentSources ON Incidents.IncidentSourceID = IncidentSources.IncidentSourceID INNER JOIN IncidentStates ON Incidents.IncidentStateID = IncidentStates.IncidentStateID INNER JOIN IncidentsTypes ON Incidents.IncidentTypeID = IncidentsTypes.IncidentTypeID  INNER JOIN Users ON Incidents.UserID = Users.UserID where Incidents.IncidentStateID!=30 Order by TimeStamp desc LIMIT 50 OFFSET 1";

            return Ok(DAL.PostGre_GetDataTable(Qry));
        }

        [HttpPost("incidentpopupsources")]
        public ActionResult PostIncidentPopupSourcesList()
        {

            string Qry = "SELECT IncidentSourceID, Name, MainExtraInfoNumber, ExtraInfo1, ExtraInfo2, ExtraInfo3, RequiresExtraInfo1, RequiresExtraInfo2, RequiresExtraInfo3 FROM IncidentSources";

            return Ok(DAL.PostGre_GetDataTable(Qry));
        }

        [HttpPost("incidenttypes")]
        public ActionResult PostIncidentTypes()
        {

            string Qry = "SELECT IncidentTypeID, Name, Priority FROM IncidentsTypes";
            return Ok(DAL.PostGre_GetDataTable(Qry));
        }

        [HttpPost("addupdateincidenttypes")]
        public ActionResult AddUpdateIncidentTypes([FromBody]JObject RqHdr)
        {

            //var incidenttypename = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["incidenttypename"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            //var incidenttypeid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["incidenttypeid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            //var userid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["userid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var incidenttypename = RqHdr["incidenttypename"].ToString();
            var incidenttypeid = Convert.ToInt32(RqHdr["incidenttypeid"].ToString());
            var userid = Convert.ToInt32(RqHdr["userid"].ToString());

            Users user = new Users();
            user.Userid = userid;
            if (user == null)
                return Ok(null);

           
            return Ok(_incTypes.AddUpdate_IncidentType(incidenttypeid, incidenttypename.ToString(), user));
        }
        [HttpPost("deleteincidenttypes")]
        public ActionResult DeleteIncidentTypes([FromBody]JObject RqHdr)
        {

            var incidenttypeid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["incidenttypeid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var userid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["userid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

            Users user = new Users();
            user.Userid = userid;
            if (user == null)
                return Ok(null);

            return Ok(_incTypes.Delete_IncidentType(incidenttypeid,  user));
        }

        [HttpPost("attachincident")]
        public ActionResult PostAttachIncident([FromBody]JObject RqHdr)
        {
            var selectedIncidentID = (RqHdr["incidentid"]);
            var mappingID = (Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["ahwalmappingid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

        
            if (selectedIncidentID == null)
            {
                return Ok(null);
            }

            if (mappingID != null)
            {
                if (mappingID.ToString().Equals(DBNull.Value) ||
                 mappingID.ToString() == "")
                {
                    return Ok(null);
                }
           
                // var mappingID = OpsLiveGrid.GetRowValues(rowIndex, "AhwalMappingID");
                var personmapping = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(em => em.Ahwalmappingid == Convert.ToInt64(mappingID));

                if (personmapping != null)
                {
                    //we have to check the current state of the person, if he is not in one of the allowed statees, we cannot handover the incident to him

                   
                    if (selectedIncidentID != null)
                    {
                        var incidentObj = _context.Incidents.FirstOrDefault<Incidents>(a => a.Incidentid == Convert.ToInt64(selectedIncidentID));
                        if (incidentObj == null)
                        {
                            return Ok(null);
                        }
                        Users user = new Users();
                        user.Userid = Convert.ToInt32(RqHdr["userid"]);

                        if (RqHdr["userid"] == null)
                        {
                            return Ok(null);
                        }
                        var result = _inc.HandOver_Incident_To_Person(user, personmapping, incidentObj);
                       return Ok(result);
                      
                    }

                }
            }

            return Ok(null);


        }

        [HttpPost("addincidents")]
        public ActionResult PostAddIncidents([FromBody]JObject RqHdr)
        {
            var incidentsourceid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["incidentsourceid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var incidenttypeid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["incidenttypeid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var incidentplace = (Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["incidentplace"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var extrainfo1 = (Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["extrainfo1"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var extrainfo2 = (Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["extrainfo1"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var extrainfo3 = (Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["extrainfo1"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var userid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["userid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

            var source = _context.Incidentsources.FirstOrDefault<Incidentsources>(a => a.Incidentsourceid == incidentsourceid);
            if (source == null)
                return Ok(null);
            Users user = new Users();
            if (user == null)
                return Ok(null);

            var newIncident = new Incidents();
            newIncident.Incidenttypeid = incidenttypeid;
            newIncident.Incidentstateid = Handler_Incidents.Incident_State_New;
            newIncident.Incidentsourceid = source.Incidentsourceid;
            newIncident.Place = incidentplace;
            if (source.Requiresextrainfo1 == 1)
            {
                newIncident.Incidentsourceextrainfo1 = extrainfo1;

            }
            if (source.Requiresextrainfo2 == 1)
            {
                newIncident.Incidentsourceextrainfo2 = extrainfo2;

            }
            if (source.Requiresextrainfo3 == 1)
            {
                newIncident.Incidentsourceextrainfo3 = extrainfo3;
            }
            var result = _inc.Add_Incident(user, newIncident);
          
            return Ok(result.Text);
        }

        [HttpPost("incidentbyid")]
        public ActionResult PostIncidentById([FromBody]JObject RqHdr)
        {
            var userid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int32>(RqHdr["userid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var incidentsourceid = (Newtonsoft.Json.JsonConvert.DeserializeObject<Int16>(RqHdr["incidentsourceid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

            var source = _context.Incidentsources.FirstOrDefault<Incidentsources>(a => a.Incidentsourceid == incidentsourceid);
          
                return Ok(source);
        }

        //[HttpPost("incidentsformappings")]
        //public ActionResult PostIncidentsForMappings()
        //{

        //    string Qry = "SELECT        Incidents.IncidentID, Incidents.IncidentStateID,Users.Name as UserName, Incidents.Place, Incidents.IncidentSourceExtraInfo1 as ExtraInfo1, Incidents.IncidentSourceExtraInfo2 as ExtraInfo2, Incidents.IncidentSourceExtraInfo3  as ExtraInfo3, Incidents.TimeStamp, Incidents.LastUpdate, Incidents.IncidentSourceID, IncidentsTypes.Name AS IncidentsTypeName, IncidentSources.Name AS IncidentSourceName, IncidentSources.ExtraInfo1 as IncidentSourceExtraInfo1, IncidentSources.ExtraInfo2 as IncidentSourceExtraInfo2, IncidentSources.ExtraInfo3 as IncidentSourceExtraInfo3 FROM   Incidents INNER JOIN IncidentSources ON Incidents.IncidentSourceID = IncidentSources.IncidentSourceID INNER JOIN IncidentStates ON Incidents.IncidentStateID = IncidentStates.IncidentStateID INNER JOIN IncidentsTypes ON Incidents.IncidentTypeID = IncidentsTypes.IncidentTypeID  INNER JOIN Users ON Incidents.UserID = Users.UserID where Incidents.IncidentStateID!=30 Order by TimeStamp desc LIMIT 50 OFFSET 1";

        //    return Ok(DAL.PostGre_GetDataTable(Qry));
        //}

        [HttpPost("opslivelist")]
        public IActionResult Postdispatchlist([FromBody]JObject RqHdr)
        {

            var ahwalid = Convert.ToInt32(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["AhwalId"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var shiftid = Convert.ToInt32(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["ShiftId"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

            String Qry = "SELECT  AhwalMapping.AhwalMappingID, AhwalMapping.AhwalID, AhwalMapping.ShiftID, AhwalMapping.SectorID, AhwalMapping.PatrolRoleID, AhwalMapping.CityGroupID, AhwalMapping.PersonID, AhwalMapping.CallerID, ";
            Qry = Qry + "   AhwalMapping.HasDevices, AhwalMapping.IncidentID, AhwalMapping.Patrolpersonstateid, AhwalMapping.LastStateChangeTimeStamp, Ranks.Name AS RankName, Ahwal.Name AS AhwalName, ";
            Qry = Qry + "     Sectors.ShortName AS SectorName, Shifts.Name AS ShiftName, Persons.MilNumber, Persons.Name AS PersonName, Persons.Mobile AS PersonMobile, HandHelds.Serial, PatrolCars.PlateNumber, ";
            Qry = Qry + "     PatrolRoles.Name AS PatrolRoleName, CityGroups.ShortName AS CityGroupName, PatrolPersonStates.Name as PatrolPersonStateName ";
            Qry = Qry + "    FROM AhwalMapping LEFT JOIN ";
            Qry = Qry + "    Persons ON AhwalMapping.PersonID = Persons.PersonID LEFT JOIN ";
            Qry = Qry + "    Ahwal ON Persons.AhwalID = Ahwal.AhwalID LEFT JOIN ";
            Qry = Qry + "     Ranks ON Persons.RankID = Ranks.RankID LEFT JOIN ";
            Qry = Qry + "     Sectors ON AhwalMapping.SectorID = Sectors.SectorID AND Ahwal.AhwalID = Sectors.AhwalID LEFT JOIN ";
            Qry = Qry + "      Shifts ON AhwalMapping.ShiftID = Shifts.ShiftID LEFT JOIN ";
            Qry = Qry + "    HandHelds ON AhwalMapping.HandHeldID = HandHelds.HandHeldID AND Ahwal.AhwalID = HandHelds.AhwalID LEFT JOIN ";
            Qry = Qry + "     PatrolCars ON AhwalMapping.PatrolID = PatrolCars.PatrolID AND Ahwal.AhwalID = PatrolCars.AhwalID LEFT JOIN ";
            Qry = Qry + "    PatrolRoles ON AhwalMapping.PatrolRoleID = PatrolRoles.PatrolRoleID LEFT JOIN ";
            Qry = Qry + "    CityGroups ON AhwalMapping.CityGroupID = CityGroups.CityGroupID AND Ahwal.AhwalID = CityGroups.AhwalID AND Sectors.SectorID = CityGroups.SectorID LEFT JOIN ";
            Qry = Qry + "    PatrolPersonStates ON AhwalMapping.Patrolpersonstateid = PatrolPersonStates.Patrolpersonstateid ";
            Qry = Qry + "    where AhwalMapping.ahwalid =" + ahwalid + " and AhwalMapping.shiftid=" + shiftid + " Order by SortingIndex asc";

            return Ok(DAL.PostGre_GetDataTable(Qry));

        }

        [HttpPost("changeopspersonstate")]
        public ActionResult PostChangeOpsPersonState([FromBody]JObject RqHdr)
        {
            var personstate = RqHdr["personstate"].ToString();
            var ahwalmappingId = Convert.ToInt64(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["ahwalmappingId"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var userid = Convert.ToInt32(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(RqHdr["userid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

          //  var user = (Users)Session["User"];
            Users user = new Users();
            user.Userid = userid;
            if (user == null)
                return Ok(null);

            var personState = new Patrolpersonstates();
            if (personstate == "Away")
            {
                personState.Patrolpersonstateid = Handler_AhwalMapping.PatrolPersonState_Away;
                var result = _ahwalmapping.Ops_ChangePersonState(user, ahwalmappingId, personState);
                return Ok(result);
            }
            else if (personstate == "Land")
            {
                personState.Patrolpersonstateid = Handler_AhwalMapping.PatrolPersonState_Land;
                var result = _ahwalmapping.Ops_ChangePersonState(user, ahwalmappingId ,personState);
                return Ok(result);

            }
            else if (personstate == "BackFromAway")
            {
                personState.Patrolpersonstateid = Handler_AhwalMapping.PatrolPersonState_Back;
                var result = _ahwalmapping.Ops_ChangePersonState(user, ahwalmappingId, personState);
                return Ok(result);
            }
            else if (personstate == "BackFromLand")
            {
                personState.Patrolpersonstateid = Handler_AhwalMapping.PatrolPersonState_Sea;
                var result = _ahwalmapping.Ops_ChangePersonState(user, ahwalmappingId, personState);
                return Ok(result);
            }
            else if (personstate == "WalkingPatrol")
            {
                personState.Patrolpersonstateid = Handler_AhwalMapping.PatrolPersonState_WalkingPatrol;
                var result = _ahwalmapping.Ops_ChangePersonState(user, ahwalmappingId, personState);
                return Ok(result);
            }
            else if (personstate == "BackFromWalking")
            {
                personState.Patrolpersonstateid = Handler_AhwalMapping.PatrolPersonState_BackFromWalking;
                var result = _ahwalmapping.Ops_ChangePersonState(user, ahwalmappingId, personState);
                return Ok(result);
            }
            return Ok(null);
            // return Ok(DAL.PostGre_GetDataTable(Qry));
        }
    }
}