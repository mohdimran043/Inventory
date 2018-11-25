using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MOI.Patrol;
namespace Core
{
    class Handler_Person
    {
        private  patrolsContext _context  = new patrolsContext();
        private  Handler_User _user = new Handler_User();

       
        public const int Rank_Lawaa = 10;
        public const int Rank_Ameed = 20;
        public const int Rank_Aqeed = 30;
        public const int Rank_Moqadam = 40;
        public const int Rank_Raaed = 50;
        public const int Rank_Naqeeb = 60;
        public const int Rank_MolazemAwal = 70;
        public const int Rank_MolazemThani = 80;
        public const int Rank_MorashehDabet = 90;
        public const int Rank_WakeelAwal = 100;
        public const int Rank_WakeelThani = 110;
        public const int Rank_Raqeeb = 120;
        public const int Rank_Naaeb = 130;
        public const int Rank_Areef = 140;
        public const int Rank_WakeelAreef = 150;
        public const int Rank_Shorti = 160;
        public const int Rank_Madani = 170;
        //all functions here require Ahwal Permssion
        //public static OperationLog Add_Person(User u, Person p)//this transaction requires User_Role_Ahwal Permisson on this AhwalID
        //{
        //    try
        //    {
        //        //first we have to check if this user is authorized to perform this transaction
        //        if (!Core.Handler_User.isAuthorized(u.UserID, p.AhwalID, Handler_User.User_Role_Ahwal))
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID; 
        //            ol_failed.OperationID = Handler_Operations.Opeartion_AddPerson;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnAuthorized;
        //            ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        //next we need to search if there is a user with same milnumber
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        Person person_exists = db.Persons.FirstOrDefault(e => e.MilNumber.Equals(p.MilNumber));
        //        if (person_exists != null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID; 
        //            ol_failed.OperationID = Handler_Operations.Opeartion_AddPerson;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "يوجد فرد بنفس الرقم العسكري: " + p.MilNumber;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        db.Persons.InsertOnSubmit(p);
        //        db.SubmitChanges();
        //        //create the opeartion log for it


        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = u.UserID; 
        //        ol_failed.OperationID = Handler_Operations.Opeartion_AddPerson;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = u.UserID; 
        //    ol.OperationID = Handler_Operations.Opeartion_AddPerson;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم اضافة الفرد: " + p.Name;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}
        //public static OperationLog Update_Person(User u, Person p)//this transaction requires User_Role_Ahwal Permisson on this AhwalID
        //{
        //    try
        //    {
        //        //first we have to check if this user is authorized to perform this transaction
        //        if (!Core.Handler_User.isAuthorized(u.UserID, p.AhwalID, Handler_User.User_Role_Ahwal))
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID;
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePerson;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnAuthorized;
        //            ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        //next we need to search if there is a user with same milnumber
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        Person person_exists = db.Persons.FirstOrDefault(e => e.PersonID.Equals(p.PersonID));
        //        if (person_exists == null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = u.UserID;
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePerson;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "لم يتم العثور على الفرد بالرقم العسكري: " + p.MilNumber;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }

        //        //we have to make sure as well thats the new milnumber is not there before
        //        if (person_exists.MilNumber != p.MilNumber)
        //        {
        //            Person same_PersonMilNumber_exists = db.Persons.FirstOrDefault(e => e.MilNumber.Equals(p.MilNumber));
        //            if (same_PersonMilNumber_exists != null)
        //            {
        //                OperationLog ol_failed = new OperationLog();
        //                ol_failed.UserID = u.UserID;
        //                ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePerson;
        //                ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //                ol_failed.Text = "يوجد فرد بنفس الرقم العسكري: " + p.MilNumber;
        //                Handler_Operations.Add_New_Operation_Log(ol_failed);
        //                return ol_failed;
        //            }
        //        }
        //        person_exists.MilNumber = p.MilNumber;
        //        person_exists.AhwalID = p.AhwalID;
        //        person_exists.Name = p.Name;
        //        person_exists.Mobile = p.Mobile;
        //        person_exists.RankID = p.RankID;
        //        person_exists.FixedCallerID = p.FixedCallerID;
        //        db.SubmitChanges();
        //        //create the opeartion log for it


        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = u.UserID;
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UpdatePerson;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = u.UserID; //admin account
        //    ol.OperationID = Handler_Operations.Opeartion_UpdatePerson;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم تعديل بيانات الفرد: " + p.Name;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}

        public  Persons GetPersonForUserForRole(Users u, long MilNumber, long roleID)
        {
            var ahwals = _user.GetUsersAuthorizedAhwalForRole(u, roleID);
            List<long> ahwalIDs = new List<long>();
            foreach (var r in ahwals)
            {
                if (!ahwalIDs.Contains(r.Ahwalid))
                    ahwalIDs.Add(r.Ahwalid);
            }
          
            var result =  _context.Persons.FirstOrDefault<Persons>(e => ahwalIDs.Contains(e.Ahwalid) && e.Milnumber == MilNumber);
            if (result != null)
            {
                return result;
            }
            return null;
        }

    }
}
