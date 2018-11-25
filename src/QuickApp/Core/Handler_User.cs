using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MOI.Patrol;

namespace Core
{
    class Handler_User
    {
        private  patrolsContext _context = new patrolsContext();

       

        // only admin can perform changes to roles or users
        public const int User_Role_Ahwal = 10;
        public const int User_Role_Maintenance  = 20;
        public const int User_Role_Ops = 30;
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        public static string GenerateHash(string text)
        {
                byte[] HashValue;
                byte[] MessageBytes = Encoding.ASCII.GetBytes(text);
                SHA512Managed SHhash = new SHA512Managed();
                string strHex = "";

                HashValue = SHhash.ComputeHash(MessageBytes);
                foreach (byte b in HashValue)
                {
                    strHex += String.Format("{0:x2}", b);
                }
                return strHex;  
        }
        //public static OperationLog Login_User(User u)
        //{
        //    DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //    User user_exists = db.Users.FirstOrDefault(e => e.UserName.Equals(u.UserName.ToLower()));
        //    if (user_exists == null)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = 1; //we should log this by admin
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UserLogin;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_LoginFail;
        //        ol_failed.Text = "لايوجد مستخدم بالاسم: " + u.UserName;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    //check if account is locked
        //    if (user_exists.AccountLocked==1)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = 1; //we should log this by admin
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UserLogin;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_LoginFail;
        //        ol_failed.Text = "تم قفل الحساب: " + u.UserName;
        //        //Handler_Operations.Add_New_Operation_Log(ol_failed);// I dont think we need to store this log
        //        return ol_failed;
        //    }
        //    //check password
        //    var correctHashedPassword = user_exists.Password;
        //    var submittedValue = user_exists.Password;
        //    // var submittedValue = Core.Handler_User.GenerateHash(u.Password + user_exists.Salt).ToUpper() ;
        //    if (correctHashedPassword != submittedValue)
        //    {
        //        //we have to log this attempt
        //        user_exists.LastFailedLogin = DateTime.Now;
        //        user_exists.FailedLogins++;
        //        user_exists.LastIPAddress = u.LastIPAddress;
        //        if (user_exists.FailedLogins > 3) //lock the account with three failed logins
        //        {
        //            user_exists.AccountLocked = 1;
        //        }
        //        db.SubmitChanges();
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = 1; //we should log this by admin
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UserLogin;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_LoginFail;
        //        ol_failed.Text = "خطا في كلمة المرور للمستخدم: " + u.UserName;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    user_exists.LastSuccessLogin = DateTime.Now;
        //    user_exists.FailedLogins=0; //reset failed logins
        //    user_exists.LastIPAddress = u.LastIPAddress;
        //    db.SubmitChanges();
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = 1; //we should log this by admin
        //    ol.OperationID = Handler_Operations.Opeartion_UserLogin;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تسجيل دخول: " + u.UserName;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}

        public  bool isAuthorized(long userid, long ahwalid, long userRoleID)
        {
            Usersrolesmap permisson_esists = _context.Usersrolesmap.FirstOrDefault(r => r.Userid == userid && r.Ahwalid == ahwalid && r.Userroleid == userRoleID);
            if (permisson_esists == null)
                return false;
            return true;
        }

        //public static User GetUserByName(string UserName)
        //{
        //    try
        //    {
        //        UserName = UserName.ToLower();
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        User user_exists = db.Users.FirstOrDefault(e => e.UserName.Equals(UserName));
        //        if (user_exists != null)
        //        {
        //            return user_exists;
        //        }

        //    }
        //    catch 
        //    {
        //    }
        //    return null;
        //}
        //public static User GetUserByID(int id)
        //{
        //    try
        //    {
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        User user_exists = db.Users.FirstOrDefault(e => e.UserID.Equals(id));
        //        if (user_exists != null)
        //        {
        //            return user_exists;
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return null;
        //}

        //public static OperationLog Add_User(User u) //admin only
        //{
        //    try
        //    {
        //        u.UserName = u.UserName.ToLower();
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        User user_exists = db.Users.FirstOrDefault(e => e.UserName.Equals(u.UserName.ToLower()));
        //        if (user_exists != null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = 1; //admin account
        //            ol_failed.OperationID = Handler_Operations.Opeartion_AddUser;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "يوجد مستخدم بنفس الاسم: " + u.UserName;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }

        //        var salt = Core.Handler_User.GetUniqueKey(16);
        //        u.Salt = salt;
        //        var password = Core.Handler_User.GenerateHash(u.Password + salt);
        //        u.Password = password.ToUpper();
        //        u.FailedLogins = 0;
        //        u.AccountLocked = 0;
        //        db.Users.InsertOnSubmit(u);
        //        db.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = 1; //admin account
        //        ol_failed.OperationID = Handler_Operations.Opeartion_AddUser;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = 1; //admin account
        //    ol.OperationID = Handler_Operations.Opeartion_AddUser;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم اضافة المستخدم: " + u.UserName;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}
        //public static OperationLog Update_User(User u)
        //{
        //    try
        //    {
        //        u.UserName = u.UserName.ToLower();
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        User user_exists = db.Users.FirstOrDefault(e => e.UserName.Equals(u.UserName.ToLower()));
        //        if (user_exists == null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = 1; //admin account
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UpdateUser;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "لم يتم العثور على مستخدم : " + u.UserName;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        user_exists.Name = u.Name;

        //        //user_exists.Password = u.Password; //we should not touch password
        //        db.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = 1; //admin account
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UpdateUser;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = 1; //admin account
        //    ol.OperationID = Handler_Operations.Opeartion_UpdateUser;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم تعديل بيانات المستخدم: " + u.UserName;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}
        //public static OperationLog User_Change_Password(User u,string oldPassword,string newPassword) //admin only
        //{
        //    try
        //    {
        //        if (newPassword.Length < 8)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = 1; //admin account
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UserUpdatePassword;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "كلمة المرور من يجب ان لاتقل عن 8 احرف";
        //           // Handler_Operations.Add_New_Operation_Log(ol_failed); no log for this
        //            return ol_failed;
        //        }
        //        u.UserName = u.UserName.ToLower();
        //        DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //        User user_exists = db.Users.FirstOrDefault(e => e.UserName.Equals(u.UserName.ToLower()));
        //        if (user_exists == null)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = 1; //admin account
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UserUpdatePassword;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "لم يتم العثور على مستخدم : " + u.UserID;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        var oldPass= Core.Handler_User.GenerateHash(oldPassword + user_exists.Salt).ToUpper();
        //        if (oldPass != user_exists.Password)
        //        {
        //            OperationLog ol_failed = new OperationLog();
        //            ol_failed.UserID = 1; //admin account
        //            ol_failed.OperationID = Handler_Operations.Opeartion_UserUpdatePassword;
        //            ol_failed.StatusID = Handler_Operations.Opeartion_Status_Failed;
        //            ol_failed.Text = "كلمة المرور القديمة غير صحيحة : " + u.UserID;
        //            Handler_Operations.Add_New_Operation_Log(ol_failed);
        //            return ol_failed;
        //        }
        //        //user_exists.UserName = u.UserName;
        //        user_exists.Salt = Core.Handler_User.GetUniqueKey(16);
        //        user_exists.Password = Core.Handler_User.GenerateHash(newPassword + user_exists.Salt).ToUpper(); 
        //        db.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        OperationLog ol_failed = new OperationLog();
        //        ol_failed.UserID = 1; //admin account
        //        ol_failed.OperationID = Handler_Operations.Opeartion_UserUpdatePassword;
        //        ol_failed.StatusID = Handler_Operations.Opeartion_Status_UnKnownError;
        //        ol_failed.Text = ex.Message;
        //        Handler_Operations.Add_New_Operation_Log(ol_failed);
        //        return ol_failed;
        //    }
        //    OperationLog ol = new OperationLog();
        //    ol.UserID = 1; //admin account
        //    ol.OperationID = Handler_Operations.Opeartion_UserUpdatePassword;
        //    ol.StatusID = Handler_Operations.Opeartion_Status_Success;
        //    ol.Text = "تم تعديل كلمة المرور للمستخدم: " + u.UserName;
        //    Handler_Operations.Add_New_Operation_Log(ol);
        //    return ol;
        //}
        //public static OperationLog Delete_User(User u)  //admin only
        //{
        //    try
        //    {

        //    }
        //    catch
        //    {

        //    }
        //    return null;
        //}
        //public static List<User> Get_All_Users()
        //{
        //    DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);
        //    return db.Users.ToList<User>();
        //}
        //public static bool Add_User_Role_Map(UsersRolesMap r) //admin only
        //{
        //    try
        //    {

        //    }
        //    catch 
        //    {

        //    }
        //    return false;
        //}
        //public static bool Remove_User_Role_Map(User u, UsersRolesMap r) //admin only
        //{
        //    try
        //    {

        //    }
        //    catch 
        //    {

        //    }
        //    return false;
        //}
        public  List<Usersrolesmap> GetUserRole(Users u)
        {
           
            var results = _context.Usersrolesmap.Where<Usersrolesmap>(e => e.Userid.Equals(u.Userid));
            if (results != null)
            {
                return results.ToList<Usersrolesmap>();
            }
            return null;

        }
        public  List<Ahwal> GetUsersAuthorizedAhwalForRole(Users u,long roleID)
        {
            var roles = GetUserRole(u);
            List<long> ids = new List<long>();
            foreach (var r in roles)
            {
                if (r.Userroleid==roleID)
                     ids.Add(r.Ahwalid);
            }
            var results = _context.Ahwal.Where<Ahwal>(e => ids.Contains(e.Ahwalid));
            if (results != null)
            {
                return results.ToList<Ahwal>();
            }
            return null;
        }
        //public static List<Ahwal> GetAllAhwals()
        //{
        //    DataClassesDataContext db = new DataClassesDataContext(Handler_Global.connectString);

        //    return db.Ahwals.ToList<Ahwal>();
           
        //}
    }

}
