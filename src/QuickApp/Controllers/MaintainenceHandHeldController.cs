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
    public class MaintainencehandheldController : Controller
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


        [HttpPost("addhandheld")]
        public ActionResult PostHandhelds([FromBody]JObject rqhdr)
        {


            var ahwalid = Convert.ToInt64(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["ahwalid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            //var barcode = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["barcode"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            var defective = Convert.ToByte(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["defective"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var serial = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["serial"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            var userid = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["userid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            var transmode = rqhdr["transmode"].ToString();
            var handheldid = Convert.ToInt64(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["handheldid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));


            Users user = new Users();
            user.Userid = Convert.ToInt32(rqhdr["userid"]);

            Handhelds h = new Handhelds();
            h.Ahwalid = ahwalid;
            h.Serial = serial;

            h.Defective = defective;
            Operationlogs result;
            if (transmode == "UPDATE")
            {
                h.Handheldid = handheldid;
                result = _handheld.Update_HandHeld(user, h);
              
            }
            else
            {
                result = _handheld.Add_HandHeldr(user, h);
              
            }

            if (result.Statusid == Handler_Operations.Opeartion_Status_Success)
            {
              
                return Ok(result.Text);
            }
            else
            {
                return Ok(result.Text);
            }
            

        }

        [HttpPost("handheldlist")]
        public ActionResult PostHandHeldList([FromBody]JObject rqhdr)
        {
            string subqry = "";
            var selectedAhwalid = Convert.ToInt32(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["ahwalid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

            if (selectedAhwalid != -1)
            {
                subqry = " and d.AhwalID = " + selectedAhwalid;
            }

            string Qry = "select d.handheldid,d.serial,d.Defective,d.BarCode,d.AhwalID,(select a.name from ahwal a where a.ahwalid = d.ahwalid ) ahwalname from handhelds d where d.serial is not null " + subqry;

            return Ok(DAL.PostGre_GetDataTable(Qry));
        }


        [HttpPost("delhandheld")]
        public ActionResult PostDeletehandheld([FromBody] JObject rqhdr)
        {
            var selectedHandheldid = Convert.ToInt32(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["handheldid"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));
            var serial = Convert.ToInt32(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(rqhdr["serial"].ToString(), new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }));

            Users user = new Users();
            user.Userid = Convert.ToInt32(rqhdr["userid"]);

            var result = _context.Handhelds.FirstOrDefault<Handhelds>(e => e.Handheldid == selectedHandheldid);
            if (result == null)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = user.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_DeleteHandHeld;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                ol_failed.Text = "لم يتم العثور على لاسلكي بالرقم: " + serial.ToString();
                _oper.Add_New_Operation_Log(ol_failed);
                return Ok(ol_failed.Text);
            }

            var ahwal_mapping_exists = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Handheldid == selectedHandheldid && e.Hasdevices == 1);
            if (ahwal_mapping_exists != null)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = user.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_DeleteHandHeld;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                ol_failed.Text = "جهاز لاسلكي قيد الاستخدام: " + serial;
                _oper.Add_New_Operation_Log(ol_failed);
                return Ok(ol_failed.Text);
            }
            
          
            _context.Handhelds.Remove(result);

            _context.SaveChanges();

            Operationlogs ol = new Operationlogs();
            ol.Userid = user.Userid;
            ol.Operationid = Handler_Operations.Opeartion_UpdateHandHeld;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم حذف لاسلكي بالرقم: " + result.Handheldid;
            _oper.Add_New_Operation_Log(ol);
            return Ok(ol.Text);

            //string Qry = "delete from handhelds  where handheldid=" + frm.Handheldid;

            //return Ok(DAL.PostGre_ExNonQry(Qry));
        }


        [HttpGet("checkinhandheldslist")]
        public ActionResult GetCheckinHandHeldList([FromQuery] int ahwalid, [FromQuery] int userid)
        {

            string subqry = "";
            subqry = " and d.ahwalid in (select ahwalid from UsersRolesMap where UserID= " + userid + " )";
            if (ahwalid != -1)
            {
                subqry = subqry + " and d.ahwalid = " + ahwalid;
            }
            String Qry = "select d.handheldid,d.serial,d.Defective,d.BarCode,d.AhwalID,(select a.name from ahwal a where a.ahwalid = d.ahwalid ) ahwalname from handhelds d where d.serial is not null AND AhwalID IN(SELECT AhwalID FROM UsersRolesMap WHERE UserID = " + userid + ") ";
             
            return Ok(DAL.PostGre_GetDataTable(Qry));

        }

        #region Hand Held Invenory
        [HttpPost("handheldinventory")]
        public ActionResult PostHandHeldInventoryList([FromBody] JObject rqhdr)
        {
            string subqry = "";
            var ahwalid = Convert.ToInt32(rqhdr["ahwalid"]);
            var stateid = Convert.ToInt32(rqhdr["stateid"]);

           subqry = " where HandHelds.AhwalID = " + ahwalid;
            

            //if (stateid != -1)
            //{
            //    subqry = " and HandHeldsCheckInOut.CheckInOutStateID = " + stateid;
            //}


            string Qry = "SELECT        HandHeldsCheckInOut.HandHeldCheckInOutID,HandHeldsCheckInOut.TimeStamp, CheckInOutStates.CheckInOutStateID,CheckInOutStates.Name AS StateName, HandHelds.AhwalID, HandHelds.Serial, Ranks.Name as PersonRank, Persons.MilNumber, Persons.Name AS PersonName ";

            Qry = Qry + " ,Ahwal.Name as ahwalname FROM Ahwal INNER JOIN";

            Qry = Qry + " HandHelds  ON Ahwal.AhwalID = HandHelds.AhwalID INNER JOIN";

            Qry = Qry + " HandHeldsCheckInOut ON HandHelds.HandHeldID = HandHeldsCheckInOut.HandHeldID INNER JOIN";

            Qry = Qry + " CheckInOutStates ON HandHeldsCheckInOut.CheckInOutStateID = CheckInOutStates.CheckInOutStateID INNER JOIN";
            Qry = Qry + " Persons ON Ahwal.AhwalID = Persons.AhwalID AND HandHeldsCheckInOut.PersonID = Persons.PersonID INNER JOIN";
            Qry = Qry + " Ranks ON Persons.RankID = Ranks.RankID  " + subqry ;

            Qry = Qry + "  ORDER BY HandHeldsCheckInOut.timestamp";

          



            return Ok(DAL.PostGre_GetDataTable(Qry));
        }

        #endregion
    }
}