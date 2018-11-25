using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Npgsql;
using MOI.Patrol.DataAccessLayer;
using CustomModels;
using Core;
using Newtonsoft.Json.Linq;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        public String constr = "server=localhost;Port=5432;User Id=postgres;password=12345;Database=Patrols";
        public DataAccess DAL = new DataAccess();
       
        /*AhwalMapping*/
        #region AhwalMapping
        [HttpGet("rolesList")]
        public List<PatrolRoles> GetRolesList()
        {

            String Qry = "SELECT PatrolRoleID, Name FROM PatrolRoles";
            return DAL.PostGre_GetData<PatrolRoles>(Qry);
        }

     
        //[HttpGet("sectorsList")]
        //public List<Sectors> GetSectorsList(int userid)
        //{
        //    String Qry = "SELECT SectorID, ShortName, CallerPrefix, Disabled,AhwalId FROM Sectors where SectorID<>1 and Disabled<>1";
        //    //String Qry = "SELECT SectorID, ShortName, CallerPrefix, Disabled,AhwalId FROM Sectors where Disabled<>1  and (AhwalID IN (SELECT AhwalID FROM UsersRolesMap WHERE (UserID = " + userid + ") ))";
        //    return DAL.PostGre_GetData<Sectors>(Qry);
        //}

        //[HttpGet("cityList")]
        //public List<CityGroups> GetCityList(int userid, int sectorid)
        //{
        //    // String Qry = "SELECT CityGroupID ,  ShortName ,  CallerPrefix ,  Disabled ,AhwalID,SectorID,Text FROM  CityGroups  where Disabled<>1 and CallerPreFix<>'0' and SectorID=" + sectorid + " and  (AhwalID IN (SELECT AhwalID FROM UsersRolesMap WHERE (UserID = " + userid + ")))";
        //    String Qry = "SELECT CityGroupID ,  ShortName ,  CallerPrefix ,  Disabled ,AhwalID,SectorID,Text FROM  CityGroups  where Disabled<>1 and CallerPreFix<>'0' and SectorID=" + sectorid;
        //    return DAL.PostGre_GetData<CityGroups>(Qry);
        //}

        //[HttpGet("associateList")]
        //public List<Associates> GetAssociateList(int userid)
        //{
        //    // String Qry = "SELECT AhwalMapping.AhwalMappingID, Persons.PersonID, Persons.MilNumber, Persons.Name FROM AhwalMapping INNER JOIN Persons ON AhwalMapping.PersonID = Persons.PersonID WHERE (AhwalMapping.PatrolRoleID <> 70) AND(AhwalMapping.AhwalID IN (SELECT AhwalMapping.AhwalID FROM UsersRolesMap WHERE (UserID = " + userid + ") ))";
        //    String Qry = "SELECT AhwalMapping.AhwalMappingID, Persons.PersonID, Persons.MilNumber, Persons.Name FROM AhwalMapping INNER JOIN Persons ON AhwalMapping.PersonID = Persons.PersonID WHERE AhwalMapping.PatrolRoleID <> 70 ";
        //    return DAL.PostGre_GetData<Associates>(Qry);
        //}

        


        public AhwalMapping GetAhwal(int ahwalmappingid)
        {
            String Qry = "SELECT lastComeBackTimeStamp,lastAwayTimeStamp,incidentID,lastLandTimeStamp,hasFixedCallerID,handHeldID,sortingIndex,sunRiseTimeStamp,sunSetTimeStamp,patrolPersonStateID,hasDevices,callerID,cityGroupID,ahwalMappingID,sectorID,patrolRoleID," +
                "shiftID, Persons.PersonID,Persons.AhwalId, Persons.MilNumber, Persons.Name as personName,Persons.RankId  FROM AhwalMapping" +
                " INNER JOIN Persons ON AhwalMapping.PersonID = Persons.PersonID where AhwalMappingId =" + ahwalmappingid;
            List<AhwalMapping> obj = DAL.PostGre_GetData<AhwalMapping>(Qry);

            if (obj.Count > 0)
            {
                return obj[0];
            }
            return null;
           

        }

      
        [HttpGet("personForUserForRole")]
        public Persons GetPersonForUserForRole(int mno, int userid)
        {
            String Qry = "SELECT PersonId, AhwalId, Name, MilNumber,RankId,Mobile,FixedCallerId FROM Persons WHERE MilNumber = " + mno;
            //String Qry = "SELECT PersonId, AhwalId, Name, MilNumber,RankId,Mobile,FixedCallerId FROM Persons WHERE AhwalID IN (SELECT AhwalID FROM UsersRolesMap WHERE UserID = " + userid + " ) and MilNumber = " + mno;
            List <Persons> obj = DAL.PostGre_GetData<Persons>(Qry);
            
            if(obj.Count > 0)
            {
                return obj[0];
            }
                return null;
        }
        public Persons GetPersonById(int personid = -1)
        {

            String Qry = "SELECT PersonId, AhwalId, Name, MilNumber,RankId,Mobile,FixedCallerId FROM Persons WHERE  PersonId = " + personid;
            List<Persons> obj = DAL.PostGre_GetData<Persons>(Qry);

            if (obj.Count > 0)
            {
                return obj[0];
            }
            return null;
        }

        [HttpGet("personsList")]
        public List<Persons> GetPersonsList(int userid)
        {
            String Qry = "SELECT PersonID, AhwalID, Name, MilNumber,RankId,Mobile,FixedCallerId FROM Persons";
            //   String Qry = "SELECT PersonID, AhwalID, Name, MilNumber,RankId,Mobile,FixedCallerId FROM Persons WHERE AhwalID IN (SELECT AhwalID FROM UsersRolesMap WHERE UserID = " + userid + " )";
            return DAL.PostGre_GetData<Persons>(Qry);
        }

      

        [HttpPost("addAhwalMapping")]
        public OperationLog PostAddAhwalMapping([FromBody]JObject data)
        {
            AhwalMapping frm = Newtonsoft.Json.JsonConvert.DeserializeObject<AhwalMapping>(data["ahwalmappingobj"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore}); 
            User u = data["userobj"].ToObject<User>();
            //  GetPerson = "";
            // string ol_failed = "";
            //OperationLog ol = new OperationLog();
            //we have to check first that this person doesn't exists before in mapping
            Persons GetPerson = GetPersonById(frm.personID);
            if (GetPerson == null)
            {
                OperationLog ol_failed = new OperationLog();
                ol_failed.userID = u.userID;
                ol_failed.operationID = Handler_Operations.Opeartion_Mapping_AddNew;
                ol_failed.statusID = Handler_Operations.Opeartion_Status_Failed;

                ol_failed.text = "لم يتم العثور على الفرد: " + frm.personID; //todo, change it actual person name
               // Handler_Operations.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }

            string person_mapping_exists = DAL.PostGre_ExScalar("select count(1) from AhwalMapping where personid = " + frm.personID);
            if (person_mapping_exists != null && person_mapping_exists != "0")
            {
                OperationLog ol_failed = new OperationLog();
                ol_failed.userID = u.userID;
                ol_failed.operationID = Handler_Operations.Opeartion_Mapping_AddNew;
                ol_failed.statusID = Handler_Operations.Opeartion_Status_Failed;
                ol_failed.text = "هذا الفرد موجود مسبقا، لايمكن اضافته مرة اخرى: " + GetPerson.milnumber.ToString() + " " + GetPerson.name;
               // Handler_Operations.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            frm.sortingIndex = 10000;
            frm.hasFixedCallerID = 0;
            if (GetPerson.fixedCallerID != null)
            { 
            if (GetPerson.fixedCallerID.Trim() != "" && GetPerson.fixedCallerID != null)
            {
                frm.hasFixedCallerID = Convert.ToByte(1);
                frm.callerID = GetPerson.fixedCallerID.Trim();
            }
            }
            //frm.sunRiseTimeStamp = null;
            //frm.sunSetTimeStamp = null;
            //frm.lastLandTimeStamp = null;
            //frm.incidentID = null;
            frm.hasDevices = 0;
            //frm.lastAwayTimeStamp = null;
            //frm.lastComeBackTimeStamp = null;
            frm.patrolPersonStateID = Handler_AhwalMapping.PatrolPersonState_None;

            string InsQry = "";
            if(frm.patrolRoleID == Handler_AhwalMapping.PatrolRole_CaptainAllSectors || frm.patrolRoleID==Handler_AhwalMapping.PatrolRole_CaptainShift)
            {
                InsQry = "insert into AhwalMapping(ahwalid,sectorid,citygroupid,shiftid,patrolroleid,personid,hasDevices," +
               "patrolPersonStateID,sortingIndex,hasFixedCallerID,callerID) values (" +
               frm.ahwalID + "," + frm.sectorID + "," + frm.cityGroupID + "," + frm.shiftID + "," + frm.patrolRoleID +
                "," + frm.personID + "," + frm.hasDevices + "," + frm.patrolPersonStateID + "," + frm.sortingIndex + "," + frm.hasFixedCallerID +
               ",'" + frm.callerID + "')";
            }
           else if (frm.patrolRoleID == Handler_AhwalMapping.PatrolRole_CaptainSector || frm.patrolRoleID == Handler_AhwalMapping.PatrolRole_SubCaptainSector)
            {
                InsQry = "insert into AhwalMapping(ahwalid,sectorid,citygroupid,shiftid,patrolroleid,personid,hasDevices," +
               "patrolPersonStateID,sortingIndex,hasFixedCallerID,callerID) values (" +
               frm.ahwalID + "," + frm.sectorID + "," + frm.cityGroupID + "," + frm.shiftID + "," + frm.patrolRoleID +
                "," + frm.personID + "," + frm.hasDevices + "," + frm.patrolPersonStateID + "," + frm.sortingIndex + "," + frm.hasFixedCallerID +
               ",'" + frm.callerID + "')";
            }
            else if (frm.patrolRoleID == Handler_AhwalMapping.PatrolRole_Associate)
            {
                InsQry = "insert into AhwalMapping(ahwalid,sectorid,citygroupid,shiftid,patrolroleid,personid,hasDevices," +
               "patrolPersonStateID,sortingIndex,hasFixedCallerID,callerID) values (" +
               frm.ahwalID + "," + frm.sectorID + "," + frm.cityGroupID + "," + frm.shiftID + "," + frm.patrolRoleID +
                "," + frm.personID + "," + frm.hasDevices + "," + frm.patrolPersonStateID + "," + frm.sortingIndex + "," + frm.hasFixedCallerID +
               ",'" + frm.callerID + "')";
            }
            else
            {
                InsQry = "insert into AhwalMapping(ahwalid,sectorid,citygroupid,shiftid,patrolroleid,personid,hasDevices," +
               "patrolPersonStateID,sortingIndex,hasFixedCallerID,callerID) values (" +
               frm.ahwalID + "," + frm.sectorID + "," + frm.cityGroupID + "," + frm.shiftID + "," + frm.patrolRoleID +
                "," + frm.personID + "," + frm.hasDevices + "," + frm.patrolPersonStateID + "," + frm.sortingIndex + "," + frm.hasFixedCallerID +
               ",'" + frm.callerID + "')";
            }

            int ret = DAL.PostGre_ExNonQry(InsQry);
            
                OperationLog ol = new OperationLog();
                 ol.userID = u.userID;
                ol.operationID = Handler_Operations.Opeartion_Mapping_AddNew;
                ol.statusID = Handler_Operations.Opeartion_Status_Success;
                ol.text = "تم اضافة الفرد: " + GetPerson.milnumber.ToString() + " " + GetPerson.name;
               // Handler_Operations.Add_New_Operation_Log(ol);
            return ol;

            
        }

        //[HttpPost("updateAhwalMapping")]
        //public int PostUpDateAhwalMapping([FromBody]AhwalMapping frm)
        //{
        //    int ret = 0;
        //    string UpdateQry = "";
        //    UpdateQry = "update AhwalMapping set ahwalid = " + frm.ahwalID + ",sectorid=" + frm.sectorID + ",citygroupid=" + frm.cityGroupID + ",shiftid=" + frm.shiftID + ",patrolroleid=" + frm.patrolRoleID + ",personid=" + frm.personID + " where ahwalmappingid = " + frm.ahwalMappingID;
        //    ret = DAL.PostGre_ExNonQry(UpdateQry);
        //    return ret;
        //}

        //[HttpDelete("deleteAhwalMapping")]
        //public OperationLog DeleteAhwalMapping([FromQuery]int ahwalMappingID , [FromQuery]int userid)
        //{
        //    //string ol_label = "";
        //    OperationLog ol = new OperationLog();
        //    int ret = 0;
        //    string DelQry = "";
        //    DelQry = "delete from AhwalMapping where ahwalMappingID = " + ahwalMappingID;
        //    ret = DAL.PostGre_ExNonQry(DelQry);
        //    if(ret > 0)
        //    {
        //        ol.userID = userid;
        //        ol.operationID = Handler_Operations.Opeartion_Mapping_Remove;
        //        ol.statusID = Handler_Operations.Opeartion_Status_Success;
        //        ol.text = "تم حذف الفرد ";  
        //        return ol;
        //    }
        //    ol.statusID = Handler_Operations.Opeartion_Status_Failed;
        //    ol.text = "Failed";
        //    return ol;
        //}


        [HttpGet("cityGroupforAhwal")]
        public List<CityGroups> GetCityGroupForAhwal(int ahwalid)
        {

            string Qry = "SELECT citygroupid, AhwalID, sectorid, shortname,callerprefix,text,disabled FROM citygroups WHERE AhwalID = " + ahwalid;
            return DAL.PostGre_GetData<CityGroups>(Qry);

        }

        [HttpGet("mappingByID")]
        public AhwalMapping GetMappingByID(int associateMapID, int userid)
        {
            // string Qry = "SELECT AhwalMapping.AhwalID, AhwalMapping.PersonID, AhwalMapping.SectorID , AhwalMapping.CityGroupID ,AhwalMapping.ShiftID  FROM AhwalMapping  WHERE AhwalMapping.AhwalID IN (SELECT AhwalMapping.AhwalID FROM UsersRolesMap WHERE (UserID = " + userid + ") and  ahwalmappingid = " + associateMapID; 
            string Qry = "SELECT AhwalMapping.AhwalID, AhwalMapping.PersonID, AhwalMapping.SectorID , AhwalMapping.CityGroupID ,AhwalMapping.ShiftID  FROM AhwalMapping  WHERE   ahwalmappingid = " + associateMapID;
            List<AhwalMapping> obj = DAL.PostGre_GetData<AhwalMapping>(Qry);

            if (obj.Count > 0)
            {
                return obj[0];
            }
            return null;
        }
        
        public AhwalMapping GetMappingByPersonId(int personid)
        {
            string Qry = "SELECT AhwalMapping.AhwalID, AhwalMapping.PersonID, AhwalMapping.SectorID , AhwalMapping.CityGroupID ,AhwalMapping.ShiftID  FROM AhwalMapping  WHERE   personid = " + personid;

            List<AhwalMapping> obj = DAL.PostGre_GetData<AhwalMapping>(Qry);

            if (obj.Count > 0)
            {
                return obj[0];
            }
            return null;
        }

        [HttpPut("updatePersonState")]
        public OperationLog updatePersonState([FromQuery]string selmenu,[FromQuery]int ahwalMappingID, [FromQuery]int userid)
        {
            //string ol_label = "";
            int PatrolPersonStateID = 0;
            if (selmenu == "غياب")
            {
                PatrolPersonStateID = Core.Handler_AhwalMapping.PatrolPersonState_Absent;
            }
            else if (selmenu == "مرضيه")
            {
                PatrolPersonStateID = Core.Handler_AhwalMapping.PatrolPersonState_Sick;
            }
            else if (selmenu == "اجازه")
            {
                PatrolPersonStateID = Core.Handler_AhwalMapping.PatrolPersonState_Off;
            }

            AhwalMapping person_mapping_exists = GetAhwal(ahwalMappingID);
           
            if (person_mapping_exists == null)
            {
                OperationLog ol_failed = new OperationLog();
                ol_failed.userID = userid;
                ol_failed.operationID = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol_failed.statusID = Handler_Operations.Opeartion_Status_Failed;
                ol_failed.text = "لم يتم العثور على التوزيع";
               // Handler_Operations.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }


            Persons GetPerson  = GetPersonById(person_mapping_exists.personID);

            if (GetPerson == null)
            {
                OperationLog ol_failed = new OperationLog();
                ol_failed.userID =userid;
                ol_failed.operationID = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol_failed.statusID = Handler_Operations.Opeartion_Status_Failed;
                ol_failed.text = "لم يتم العثور على الفرد: " + person_mapping_exists.personID; //todo, change it actual person name
              //  Handler_Operations.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }

            //last check
            //if he has devices, dont change his state to anything
            if (Convert.ToBoolean(person_mapping_exists.hasDevices))
            {

                OperationLog ol_failed = new OperationLog();
                ol_failed.userID = userid;
                ol_failed.operationID = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol_failed.statusID = Handler_Operations.Opeartion_Status_Failed;
                ol_failed.text = "هذا المستخدم يملك حاليا اجهزة";
               // Handler_Operations.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }

            OperationLog ol = new OperationLog();
            int ret = 0;
            string Qry = "";
            Qry = "update AhwalMapping set PatrolPersonStateID = " + PatrolPersonStateID + " , LastStateChangeTimeStamp = '" + DateTime.Now + "' where AhwalMappingId = " + ahwalMappingID;
            ret = DAL.PostGre_ExNonQry(Qry);
            Qry = "insert into patrolpersonstatelog (UserID,PatrolPersonStateID,TimeStamp,PersonID) values(" + userid + " , " + PatrolPersonStateID + " , '" + DateTime.Now + "' , " + person_mapping_exists.personID + ")";
            ret = DAL.PostGre_ExNonQry(Qry);
            if (ret > 0)
            {
                ol.userID = userid;
                ol.operationID = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol.statusID = Handler_Operations.Opeartion_Status_Success;
                ol.text = "احوال تغيير حالة الفرد " + GetPerson.milnumber + " " + GetPerson.name ;
                return ol;
            }
            ol.statusID = Handler_Operations.Opeartion_Status_Failed;
            ol.text = "Failed";
            return ol;
        }



        #endregion

    }
}