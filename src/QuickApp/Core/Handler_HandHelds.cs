using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MOI.Patrol;
namespace Core
{
    class Handler_HandHelds
    {
        private patrolsContext _context = new patrolsContext();
        private Handler_User _user = new Handler_User();
        private Handler_Operations _oper = new Handler_Operations();


        public List<Handhelds> GetHandHeldsForUserForRole(Users u, long roleID)
        {
            var ahwals = _user.GetUsersAuthorizedAhwalForRole(u, roleID);
            List<long> ahwalIDs = new List<long>();
            foreach (var r in ahwals)
            {
                if (!ahwalIDs.Contains(r.Ahwalid))
                    ahwalIDs.Add(r.Ahwalid);
            }
            
            var results =  _context.Handhelds.Where<Handhelds>(e => ahwalIDs.Contains(e.Ahwalid) && e.Serial != "NONE");
            if (results != null)
            {
                return results.ToList<Handhelds>();
            }
            return null;
        }
        public  Handhelds GetHandHeldBySerialForUserForRole(Users u, string serial, long roleID)
        {
            var ahwals = _user.GetUsersAuthorizedAhwalForRole(u, roleID);
            List<long> ahwalIDs = new List<long>();
            foreach (var r in ahwals)
            {
                if (!ahwalIDs.Contains(r.Ahwalid))
                    ahwalIDs.Add(r.Ahwalid);
            }

            var result = _context.Handhelds.FirstOrDefault<Handhelds>(e => ahwalIDs.Contains(e.Ahwalid) && e.Serial == serial);
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public  Handhelds GetHandHeldByID(Users u, Handhelds h)
        {

            
            //first we have to check if this Users is authorized to perform this transaction
            if (!_user.isAuthorized(u.Userid, h.Ahwalid, Handler_User.User_Role_Ahwal))
            {

                return null; //we dont need to log this since its just read operation
            }
            var result = _context.Handhelds.FirstOrDefault<Handhelds>(e => e.Handheldid == h.Handheldid);
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public  List<Handhelds> GetAllHandHelds(Users u)//TODO: Apply permisson checking
        {
         

            return _context.Handhelds.ToList<Handhelds>();

        }

        //all functions here require Ahwal Permssion
        public  Operationlogs Add_HandHeldr(Users u, Handhelds h)//this transaction requires User_Role_Maintenance Permisson on this AhwalID
        {
            try
            {
                //first we have to check if this Users is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, h.Ahwalid, Handler_User.User_Role_Maintenance))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid  = Handler_Operations.Opeartion_AddHandHeld;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}
                //next we need to search if there is a Handhelds with same serial
                Handhelds HandHeld_exists =  _context.Handhelds.FirstOrDefault(e => e.Serial.Equals(h.Serial));
                if (HandHeld_exists != null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid  = Handler_Operations.Opeartion_AddHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "يوجد لاسلكي بنفس رقم : " + h.Serial;
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                h.Barcode = "HAN" + h.Serial;
              //  var handhelds = _context.Set<Handhelds>();
              //  handhelds.Add(h);

               _context.Handhelds.Add(h);
                 _context.SaveChanges();

            }

            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid  = Handler_Operations.Opeartion_AddHandHeld;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            Operationlogs ol = new Operationlogs();
            ol.Userid = u.Userid;
            ol.Operationid  = Handler_Operations.Opeartion_AddHandHeld;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم اضافة لاسلكي: " + h.Serial;
            _oper.Add_New_Operation_Log(ol);
            return ol;
        }
        public  Operationlogs Update_HandHeld(Users u, Handhelds h)//this transaction requires User_Role_Maintenance Permisson on this AhwalID
        {
            try
            {
                //first we have to check if this Users is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, h.Ahwalid, Handler_User.User_Role_Maintenance))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid  = Handler_Operations.Opeartion_UpdateHandHeld;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}
                //next we need to search if there is a Handhelds car with same serial
                Handhelds HandHeld_exists =  _context.Handhelds.FirstOrDefault(e => e.Handheldid.Equals(h.Handheldid));
                if (HandHeld_exists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid  = Handler_Operations.Opeartion_UpdateHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على لاسلكي بالرقم: " + h.Serial;
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //we have to make sure as well thats the new serial is not there before
                if (HandHeld_exists.Serial != h.Serial)//in case only the Users did choose new serial
                {
                    Handhelds same_HandHeld_serial =  _context.Handhelds.FirstOrDefault(e => e.Serial.Equals(h.Serial));
                    if (same_HandHeld_serial != null)
                    {
                        Operationlogs ol_failed = new Operationlogs();
                        ol_failed.Userid = u.Userid;
                        ol_failed.Operationid  = Handler_Operations.Opeartion_UpdateHandHeld;
                        ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                        ol_failed.Text = "يوجد لاسلكي بنفس رقم : " + h.Serial;
                         _oper.Add_New_Operation_Log(ol_failed);
                        return ol_failed;
                    }
                }

                HandHeld_exists.Serial = h.Serial;
                HandHeld_exists.Barcode = "HAN" + h.Serial; //just to make sure no one miss this up even me
                HandHeld_exists.Defective = h.Defective;
                HandHeld_exists.Ahwalid = h.Ahwalid; //we are allowing changing of AhwalID for Patrol Cars
                 _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid  = Handler_Operations.Opeartion_UpdateHandHeld;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                 _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            Operationlogs ol = new Operationlogs();
            ol.Userid = u.Userid;
            ol.Operationid  = Handler_Operations.Opeartion_UpdateHandHeld;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم تعديل بيانات اللاسلكي: " + h.Handheldid;
             _oper.Add_New_Operation_Log(ol);
            return ol;
        }
    }
}
