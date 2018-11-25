using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MOI.Patrol;

namespace Core
{
    class Handler_PatrolCars //nothing done yet here
    {
        private patrolsContext _context = new patrolsContext();
        private Handler_User _user = new Handler_User();

        //all functions here require Ahwal Permssion
        //public static OperationLog Add_PatrolCar(User u, PatrolCar p)//this transaction requires User_Role_Maintenance Permisson on this AhwalID
        //{
        //    try
        //    {
        //        //first we have to check if this user is authorized to perform this transaction
        //        if (!Core.Handler_User.isAuthorized(u.UserID, p.AhwalID, Handler_User.User_Role_Maintenance))
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID;
        //            ol_failed.OperationID = Handler_Operations.Opeartion_AddPatrolCar;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnAuthorized;
        //            ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        //next we need to search if there is a PatrolCar with same PlateNumber
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        PatrolCar PatrolCar_exists = db.PatrolCars.FirstOrDefault(e => e.PlateNumber.Equals(p.PlateNumber));
        //        if (PatrolCar_exists != null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID;
        //            ol_failed.OperationID = Handler_Operations.Opeartion_AddPatrolCar;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "يوجد دورية بنفس رقم اللوحة: " + p.PlateNumber;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        p.BarCode = "PAT" + p.PlateNumber;
        //        db.PatrolCars.InsertOnSubmit(p);
        //        db.SubmitChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = u.UserID;
        //        ol_failed.OperationID = Handler_Operations.Opeartion_AddPatrolCar;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = u.UserID; //admin account
        //    ol.OperationID = Handler_Operations.Opeartion_AddPatrolCar;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم اضافة الدورية: " + p.PlateNumber;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}
        //public static OperationLog Update_PatrolCar(User u, PatrolCar p)//this transaction requires User_Role_Maintenance Permisson on this AhwalID
        //{
        //    try
        //    {
        //        //first we have to check if this user is authorized to perform this transaction
        //        if (!Core.Handler_User.isAuthorized(u.UserID, p.AhwalID, Handler_User.User_Role_Maintenance))
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID;
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePatrolCar;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnAuthorized;
        //            ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        //next we need to search if there is a Patrol car with same PlateNumber
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        PatrolCar PatrolCar_exists = db.PatrolCars.FirstOrDefault(e => e.PatrolID.Equals(p.PatrolID));
        //        if (PatrolCar_exists == null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID;
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePatrolCar;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "لم يتم العثور على دورية بالرقم: " + p.PlateNumber;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        //we have to make sure as well thats the new platenumber is not there before
        //        if (PatrolCar_exists.PlateNumber != p.PlateNumber)
        //        {
        //            PatrolCar same_PatrolCarPlate_exists = db.PatrolCars.FirstOrDefault(e => e.PlateNumber.Equals(p.PlateNumber));
        //            if (same_PatrolCarPlate_exists != null)
        //            {
        //                OperationLog ol_failed = new OperationLog();
        //                ol_failed.UserID = u.UserID;
        //                ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePatrolCar;
        //                ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //                ol_failed.Text = "يوجد دورية بنفس رقم اللوحة: " + p.PlateNumber;
        //                Handler_Operations.Add_New_Operation_Log(ol_failed);
        //                return ol_failed;
        //            }
        //        }

        //        PatrolCar_exists.PlateNumber = p.PlateNumber;
        //        PatrolCar_exists.Model = p.Model;
        //        PatrolCar_exists.Type = p.Type;
        //        PatrolCar_exists.BarCode = "PAT" + p.PlateNumber; //just to make sure no one miss this up even me
        //        PatrolCar_exists.Defective = p.Defective;
        //        PatrolCar_exists.Rental = p.Rental;
        //        PatrolCar_exists.VINNumber = p.VINNumber;
        //        PatrolCar_exists.AhwalID = p.AhwalID; //we are allowing changing of AhwalID for Patrol Cars
        //        db.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = u.UserID;
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePatrolCar;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = u.UserID; //admin account
        //    ol.OperationID = Handler_Operations.Opeartion_UpdatePatrolCar;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم تعديل بيانات الدورية: " + p.PlateNumber;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}
        public  List<Patrolcars> GetPatrolCarsForUserForRole(Users u, long roleID)
        {
            var ahwals = _user.GetUsersAuthorizedAhwalForRole(u, roleID);
            List<long> ahwalIDs = new List<long>();
            foreach (var r in ahwals)
            {
                if (!ahwalIDs.Contains(r.Ahwalid))
                    ahwalIDs.Add(r.Ahwalid);
            }
            
            var results = _context.Patrolcars.Where<Patrolcars>(e => ahwalIDs.Contains(e.Ahwalid) && e.Platenumber != "NONE");
            if (results != null)
            {
                return results.ToList<Patrolcars>();
            }
            return null;
        }

        public Patrolcars GetPatrolCarByPlateNumberForUserForRole(Users u, string platenumber, long roleID)
        {
            var ahwals = _user.GetUsersAuthorizedAhwalForRole(u, roleID);
            List<long> ahwalIDs = new List<long>();
            foreach (var r in ahwals)
            {
                if (!ahwalIDs.Contains(r.Ahwalid))
                    ahwalIDs.Add(r.Ahwalid);
            }
            
            var result = _context.Patrolcars.FirstOrDefault<Patrolcars>(e => ahwalIDs.Contains(e.Ahwalid) && e.Platenumber == platenumber);
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public  Patrolcars GetPatrolCardByID(Users u, Patrolcars p)
        {

            //first we have to check if this user is authorized to perform this transaction
            if (!_user.isAuthorized(u.Userid, p.Ahwalid, Handler_User.User_Role_Ahwal))
            {

                return null; //we dont need to log this since its just read operation
            }
            var result = _context.Patrolcars.FirstOrDefault<Patrolcars>(e => e.Patrolid == p.Patrolid);
            if (result != null)
            {
                return result;
            }
            return null;
        }

    }
}
