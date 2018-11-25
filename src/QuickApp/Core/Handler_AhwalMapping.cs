using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOI.Patrol;
using MOI.Patrol.DataAccessLayer;

namespace Core
{
    public class Handler_AhwalMapping
    {
        private patrolsContext _context = new patrolsContext();
        private Handler_User _user = new Handler_User();
        private Handler_Operations _oper = new Handler_Operations();
        private DataAccess DAL = new DataAccess();

        public const int PatrolPersonState_None = 10;

        public const int PatrolPersonState_SunRise = 20;
        public const int PatrolPersonState_Sea = 30;
        public const int PatrolPersonState_Back = 40;
        public const int PatrolPersonState_BackFromWalking = 74;

        public const int PatrolPersonState_SunSet = 50;
        public const int PatrolPersonState_Away = 60;
        public const int PatrolPersonState_Land = 70;
        public const int PatrolPersonState_WalkingPatrol = 72;

        public const int PatrolPersonState_Absent = 80;
        public const int PatrolPersonState_Off = 90;
        public const int PatrolPersonState_Sick = 100;



        public const int CheckInState = 10;
        public const int CheckOutState = 20;


        //patrol roles
        public const int PatrolRole_None = 0;
        public const int PatrolRole_CaptainAllSectors = 10;
        public const int PatrolRole_CaptainShift = 20;
        public const int PatrolRole_CaptainSector = 30;
        public const int PatrolRole_SubCaptainSector = 40;
        public const int PatrolRole_CityGroupOfficer = 50;
        public const int PatrolRole_PatrolPerson = 60;
        public const int PatrolRole_Associate = 70;
        public const int PatrolRole_Temp = 80;
        //public const int PatrolRole_TodaysOff = 70;
        //public const int PatrolRole_Absent = 80;
        //public const int PatrolRole_SickLeave = 90;
        //shifts
        public const int Shifts_Morning = 1;
        public const int Shifts_Afternoon = 2;
        public const int Shifts_Night = 3;

        //sectors 
        public const int Sector_Public = 1;
        public const int Sector_First = 2;
        public const int Sector_Second = 3;
        public const int Sector_Third = 4;
        public const int Sector_Fourth = 5;
        public const int Sector_Fifth = 6;
        public const int Sector_North = 7;
        public const int Sector_South = 8;
        public const int Sector_West = 9;

        public Ahwalmapping GetMappingByID(Users u, long mappingID, long roleID)
        {
            var ahwals = _user.GetUsersAuthorizedAhwalForRole(u, roleID);
            List<long> Ahwalids = new List<long>();
            foreach (var r in ahwals)
            {
                if (!Ahwalids.Contains(r.Ahwalid))
                    Ahwalids.Add(r.Ahwalid);
            }

            var result = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => Ahwalids.Contains(e.Ahwalid) && e.Ahwalmappingid == mappingID);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public  Ahwalmapping GetMappingByPersonID(Users u, Persons p)
        {

            //first we have to check if this user is authorized to perform this transaction
            if (!_user.isAuthorized(u.Userid, p.Ahwalid, Handler_User.User_Role_Ahwal))
            {

                return null; //we dont need to log this since its just read operation
            }
            var result = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Personid == p.Personid);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public  Operationlogs CheckOutPatrolAndHandHeld(Users u, Ahwalmapping m, Patrolcars p, Handhelds h)
        {
            try
            {
                //first we have to check if this user is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, p.Ahwalid, Handler_User.User_Role_Ahwal))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}
                //we have to check first that this person doesn't exists before in mapping
                var person_mapping_exists = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Ahwalmappingid.Equals(m.Ahwalmappingid));
                if (person_mapping_exists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على التوزيع";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                var GetPerson = _context.Persons.FirstOrDefault<Persons>(e => e.Personid.Equals(m.Personid));
                if (GetPerson == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على الفرد: " + m.Personid; //todo, change it actual person name
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                if (!Convert.ToBoolean(person_mapping_exists.Hasdevices))
                {

                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "هذا الفرد لايملك حاليا اجهزة";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                //NEW Added 16/7/2017 - we have to make sure that we cannot sunset someone who currently has an incident
                if (person_mapping_exists.Incidentid != null && !person_mapping_exists.Incidentid.Equals(DBNull.Value))
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "الدورية مستلمه لازالت مستلمه بلاغ-خاطب العمليات";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }


                person_mapping_exists.Hasdevices = Convert.ToByte(0);
                person_mapping_exists.Patrolpersonstateid = Core.Handler_AhwalMapping.PatrolPersonState_SunSet;
                person_mapping_exists.Sunsettimestamp = DateTime.Now;
                person_mapping_exists.Laststatechangetimestamp = DateTime.Now;



                _context.SaveChanges();
                //log it

                //we have to add this record in checkIn and CheckOut Table
                var PatrolCheckOutLog = new Patrolcheckinout();
                PatrolCheckOutLog.Checkinoutstateid = Core.Handler_AhwalMapping.CheckOutState;
                PatrolCheckOutLog.Timestamp = DateTime.Now;
                PatrolCheckOutLog.Personid = m.Personid;
                PatrolCheckOutLog.Patrolid = p.Patrolid;
                // _context.Patrolcheckinout.Add(PatrolCheckOutLog);
                _context.Patrolcheckinout.Add(PatrolCheckOutLog);

                 var HandHeldCheckOutLog = new Handheldscheckinout();
                HandHeldCheckOutLog.Checkinoutstateid = Core.Handler_AhwalMapping.CheckOutState;
                HandHeldCheckOutLog.Timestamp = DateTime.Now;
                HandHeldCheckOutLog.Personid = m.Personid;
                HandHeldCheckOutLog.Handheldid = h.Handheldid;
                _context.Handheldscheckinout.Add(HandHeldCheckOutLog);

                _context.SaveChanges();

                //record this in personstatechangelog
                var personStateLog = new Patrolpersonstatelog();
                personStateLog.Userid = u.Userid;
                personStateLog.Patrolpersonstateid = Core.Handler_AhwalMapping.PatrolPersonState_SunSet;
                personStateLog.Timestamp = DateTime.Now;
                personStateLog.Personid = m.Personid;
                LogPersonStateChange(personStateLog);

                Operationlogs ol = new Operationlogs();
                ol.Userid = u.Userid;
                ol.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                ol.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol.Text = "تم الاستلام من الفرد: " + GetPerson.Milnumber.ToString() + " " + GetPerson.Name +
                    "  الدورية رقم: " + p.Platenumber + " والجهاز رقم: " + h.Serial;
                _oper.Add_New_Operation_Log(ol);

                return ol;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
        }

        public  void LogPersonStateChange(Patrolpersonstatelog p)
        {
            
            _context.Patrolpersonstatelog.Add(p);
            _context.SaveChanges();
        }

        public  bool PatrolCarWithSomeOneElse(Users u, long WantToHavePersonID, long WantedPatrolID)
        {
            var result = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Hasdevices == 1 && e.Personid != WantToHavePersonID && e.Patrolroleid == WantedPatrolID);
            if (result == null)
            {
                return false;
            }
            return true;
        }
        public  bool HandHeldWithSomeOneElse(Users u, long WantToHavePersonID, long WantedHandHeldID)
        {
            //check if there is a user, who still have it, and its not the same user asking for it
            var result = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Hasdevices == 1 && e.Personid != WantToHavePersonID && e.Handheldid == WantedHandHeldID);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public  Operationlogs CheckInPatrolAndHandHeld(Users u, Ahwalmapping m, Patrolcars p, Handhelds h)
        {
            try
            {
                //first we have to check if this user is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, p.Ahwalid, Handler_User.User_Role_Ahwal))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckInPatrolAndHandHeld;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}
                //we have to check first that this person doesn't exists before in mapping

                var person_mapping_exists = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Ahwalmappingid.Equals(m.Ahwalmappingid));
                if (person_mapping_exists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckInPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على التوزيع";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                var GetPerson = _context.Persons.FirstOrDefault<Persons>(e => e.Personid.Equals(m.Personid));
                if (GetPerson == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckInPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على الفرد: " + m.Personid; //todo, change it actual person name
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                if (Convert.ToBoolean(person_mapping_exists.Hasdevices))
                {

                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckInPatrolAndHandHeld;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "هذا المستخدم يملك حاليا اجهزة";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                person_mapping_exists.Patrolid = p.Patrolid;
                person_mapping_exists.Handheldid = h.Handheldid;
                person_mapping_exists.Patrolpersonstateid = Core.Handler_AhwalMapping.PatrolPersonState_SunRise;
                person_mapping_exists.Sunrisetimestamp = DateTime.Now;
                person_mapping_exists.Sunsettimestamp = null;//we have to reset this time
                person_mapping_exists.Hasdevices = Convert.ToByte(1);
                person_mapping_exists.Laststatechangetimestamp = DateTime.Now;
                _context.SaveChanges();
                //log it

                //we have to add this record in checkIn and CheckOut Table
                var PatrolCheckInLog = new Patrolcheckinout();
                PatrolCheckInLog.Checkinoutstateid = Core.Handler_AhwalMapping.CheckInState;
                PatrolCheckInLog.Timestamp = DateTime.Now;
                PatrolCheckInLog.Personid = m.Personid;
                PatrolCheckInLog.Patrolid = p.Patrolid;
                _context.Patrolcheckinout.Add(PatrolCheckInLog);

                var HandHeldCheckInLog = new Handheldscheckinout();
                HandHeldCheckInLog.Checkinoutstateid = Core.Handler_AhwalMapping.CheckInState;
                HandHeldCheckInLog.Timestamp = DateTime.Now;
                HandHeldCheckInLog.Personid = m.Personid;
                HandHeldCheckInLog.Handheldid = h.Handheldid;
                _context.Handheldscheckinout.Add(HandHeldCheckInLog);

                _context.SaveChanges();

                //record this in personstatechangelog
                var personStateLog = new Patrolpersonstatelog();
                personStateLog.Userid = u.Userid;
                personStateLog.Patrolpersonstateid = Core.Handler_AhwalMapping.PatrolPersonState_SunRise;
                personStateLog.Timestamp = DateTime.Now;
                personStateLog.Personid = m.Personid;
                LogPersonStateChange(personStateLog);

                Operationlogs ol = new Operationlogs();
                ol.Userid = u.Userid;
                ol.Operationid = Handler_Operations.Opeartion_Mapping_CheckInPatrolAndHandHeld;
                ol.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol.Text = "تم تسليم الفرد: " + GetPerson.Milnumber.ToString() + " " + GetPerson.Name +
                    "  الدورية رقم: " + p.Platenumber + " والجهاز رقم: " + h.Serial;
                _oper.Add_New_Operation_Log(ol);

                return ol;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_CheckOutPatrolAndHandHeld;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
        }

        public  Operationlogs AddNewMapping(Users u, Ahwalmapping m) //requires AhwalRole
        {
            try
            {
                //first we have to check if this user is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, m.Ahwalid, Handler_User.User_Role_Ahwal))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_AddNew;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}
                //we have to check first that this person doesn't exists before in mapping
                var GetPerson = _context.Persons.FirstOrDefault<Persons>(e => e.Personid.Equals(m.Personid));
                if (GetPerson == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_AddNew;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على الفرد: " + m.Personid; //todo, change it actual person name
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                var person_mapping_exists = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Personid.Equals(m.Personid));
                if (person_mapping_exists != null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_AddNew;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "هذا الفرد موجود مسبقا، لايمكن اضافته مرة اخرى: " + GetPerson.Milnumber.ToString() + " " + GetPerson.Name;
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
               
                m.Sortingindex = 10000;
                //m.Patrolid = null;
                //m.Handheldid = null;
                ////force Sector to Public and CityGroup to None for PatrolRole_CaptainAllSectors and PatrolRole_CaptainShift
                //if (m.Patrolroleid == Core.Handler_AhwalMapping.PatrolRole_CaptainAllSectors ||
                //m.Patrolroleid == Core.Handler_AhwalMapping.PatrolRole_CaptainShift)
                //{
                //    m.SectorID = Core.Handler_AhwalMapping.Sector_Public;
                //    m.CityGroupID = Core.Handler_AhwalMapping.CityGroup_None;
                //}
                ////force citygroup to be None for Sector Captain and SubCaptain
                //if (m.Patrolroleid == Core.Handler_AhwalMapping.PatrolRole_CaptainSector ||
                //    m.Patrolroleid == Core.Handler_AhwalMapping.PatrolRole_SubCaptainSector)
                //    m.CityGroupID = Core.Handler_AhwalMapping.CityGroup_None;
                
                if (GetPerson.Fixedcallerid.Trim() != "" && GetPerson.Fixedcallerid != null)
                {
                    m.Hasfixedcallerid = Convert.ToByte(1);
                    m.Callerid = GetPerson.Fixedcallerid.Trim();
                }
                //we have to check as well, if we have already 10 people within same citygroup
                //we cannot add more than 10 per group
                var totalInSame = _context.Ahwalmapping.Where<Ahwalmapping>(e => e.Ahwalid == m.Ahwalid
                && e.Shiftid == m.Shiftid && e.Sectorid == m.Sectorid
                && e.Citygroupid == m.Citygroupid
                && e.Patrolroleid != Core.Handler_AhwalMapping.PatrolRole_Associate
                && e.Patrolroleid != Core.Handler_AhwalMapping.PatrolRole_CaptainAllSectors
                && e.Patrolroleid != Core.Handler_AhwalMapping.PatrolRole_CaptainShift);
                if (totalInSame != null)
                {
                    if (totalInSame.Count<Ahwalmapping>() >= 10)
                    {
                        Operationlogs ol_failed = new Operationlogs();
                        ol_failed.Userid = u.Userid;
                        ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_AddNew;
                        ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                        ol_failed.Text = "لايمكن اضافة اكثر من عشر اشخاص في نفس المنطقة";
                        _oper.Add_New_Operation_Log(ol_failed);
                        return ol_failed;
                    }
                }
               // m.Sector = _context.Sectors.f;
                //m.Sunrisetimestamp = null;
                //m.Sunsettimestamp = null;
                //m.Lastlandtimestamp = null;
                //m.Incidentid = null;
                m.Hasdevices = 0;
                //m.Lastawaytimestamp = null;
                //m.Lastcomebacktimestamp = null;
                m.Patrolpersonstateid = Core.Handler_AhwalMapping.PatrolPersonState_None;
                //if (m.Sectorid !=null)
                //{
                //   m.Sector = _context.Sectors.FirstOrDefault<Sectors>(ec => ec.Sectorid == m.Sectorid);

                //}


                //if (m.Citygroupid != null)
                //{
                //    m.Citygroup = _context.Citygroups.FirstOrDefault<Citygroups>(ec => ec.Citygroupid == m.Citygroupid);

                //}


                //if (m.Shiftid != null)
                //{
                //    m.Shift = _context.Shifts.FirstOrDefault<Shifts>(ec => ec.Shiftid == m.Shiftid);

                //}


                //if (m.Patrolroleid != null)
                //{

                //    m.Patrolrole = _context.Patrolroles.FirstOrDefault<Patrolroles>(ec => ec.Patrolroleid == m.Patrolroleid);

                //}


                //  m.Patrolpersonstate = _context.Patrolpersonstates.FirstOrDefault<Patrolpersonstates>(ec => ec.Patrolpersonstateid == m.Patrolpersonstateid);
                /// m.Person = _context.Persons.FirstOrDefault<Persons>(ec => ec.Personid == m.Personid);
                string InsQry = "";
                InsQry = "insert into AhwalMapping(ahwalid,sectorid,citygroupid,shiftid,patrolroleid,personid,hasDevices," +
                              "patrolPersonStateID,sortingIndex,hasFixedCallerID,callerID) values (" +
                              m.Ahwalid + "," + m.Sectorid + "," + m.Citygroupid + "," + m.Shiftid + "," + m.Patrolid +
                               "," + m.Personid + "," + m.Hasdevices + "," + m.Patrolpersonstateid + "," + m.Sortingindex + "," + m.Hasfixedcallerid +
                              ",'" + m.Callerid + "')";
                int ret = DAL.PostGre_ExNonQry(InsQry);

              //  _context.Ahwalmapping.Add(m);
             //   _context.SaveChanges();

                //time to resort sortingindex
                // Core.Handler_AhwalMapping.ReSortMappings();
                //log it
                Operationlogs ol = new Operationlogs();
                ol.Userid = u.Userid;
                ol.Operationid = Handler_Operations.Opeartion_Mapping_AddNew;
                ol.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol.Text = "تم اضافة الفرد: " + GetPerson.Milnumber.ToString() + " " + GetPerson.Name;
                _oper.Add_New_Operation_Log(ol);
                return ol;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_AddNew;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
        }



        public Operationlogs Ahwal_ChangePersonState(Users u, long mappingID, Patrolpersonstates s)
        {
            try
            {
                //we have to check first that this person doesn't exists before in mapping
                var person_mapping_exists = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Ahwalmappingid.Equals(mappingID));
                if (person_mapping_exists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على التوزيع";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //a bit different here, for Ahwalmapping, allowed states are only for sick,leave,sunrise,sunset
                int[] allowedState = {Core.Handler_AhwalMapping.PatrolPersonState_SunRise,
                Core.Handler_AhwalMapping.PatrolPersonState_SunSet,
                Core.Handler_AhwalMapping.PatrolPersonState_Off,
                Core.Handler_AhwalMapping.PatrolPersonState_Sick,
                Core.Handler_AhwalMapping.PatrolPersonState_Absent};
                //first we have to check if this user is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, person_mapping_exists.Ahwalid, Handler_User.User_Role_Ahwal)
                //    && !allowedState.Contains(s.Patrolpersonstateid))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}

                var GetPerson = _context.Persons.FirstOrDefault<Persons>(e => e.Personid.Equals(person_mapping_exists.Personid));
                if (GetPerson == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على الفرد: " + person_mapping_exists.Personid; //todo, change it actual person name
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //last check
                //if he has devices, dont change his state to anything
                if (Convert.ToBoolean(person_mapping_exists.Hasdevices))
                {

                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "هذا المستخدم يملك حاليا اجهزة";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                //in ahwal mapping, we can change associate person state to sick, off,leave
                person_mapping_exists.Patrolpersonstateid = s.Patrolpersonstateid;
                person_mapping_exists.Laststatechangetimestamp = DateTime.Now;
                _context.SaveChanges();
                //log it
                //record this in personstatechangelog
                var personStateLog = new Patrolpersonstatelog();
                personStateLog.Userid = u.Userid;
                personStateLog.Patrolpersonstateid = s.Patrolpersonstateid;
                personStateLog.Timestamp = DateTime.Now;
                personStateLog.Personid = person_mapping_exists.Personid;
               LogPersonStateChange(personStateLog);



                Operationlogs ol = new Operationlogs();
                ol.Userid = u.Userid;
                ol.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol.Text = "احوال تغيير حالة الفرد: " + GetPerson.Milnumber + " " + GetPerson.Name;
                _oper.Add_New_Operation_Log(ol);

                return ol;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
        }

        public  Operationlogs DeleteMapping(Users u, long mappingID)
        {
            try
            {
                var result = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Ahwalmappingid == mappingID);
                if (result == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Update;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على التوزيع: " + mappingID.ToString();
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //first we have to check if this user is authorized to perform this transaction
                if (!_user.isAuthorized(u.Userid, result.Ahwalid, Handler_User.User_Role_Ahwal))
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Remove;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                var GetPerson = _context.Persons.FirstOrDefault<Persons>(e => e.Personid.Equals(result.Personid));
                if (GetPerson == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Update;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على الفرد: " + result.Personid; //todo, change it actual person name
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //if he has devices, we cannot remove him

                //I'll disable this check for now, we still can delete a user even if he has devices
                if (Convert.ToBoolean(result.Hasdevices))
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Remove;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = ":هذا الفرد لم يسلم الاجهزة " + GetPerson.Milnumber + " " + GetPerson.Name;
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //if he has someone associate to him,we cannot delete him, must delete the associate first

                var hasAssociate = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Associtatepersonid == result.Personid);
                if (hasAssociate != null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Remove;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "هذا المستخدم لديه مرافق" + GetPerson.Milnumber + " " + GetPerson.Name;//TODO: Add associate details
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                _context.Ahwalmapping.Remove(result);

                _context.SaveChanges();

                //we need to resort
              //  Core.Handler_AhwalMapping.ReSortMappings();
                Operationlogs ol = new Operationlogs();
                ol.Userid = u.Userid;
                ol.Operationid = Handler_Operations.Opeartion_Mapping_Remove;
                ol.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol.Text = "تم حذف الفرد: " + GetPerson.Milnumber.ToString() + " " + GetPerson.Name;
                _oper.Add_New_Operation_Log(ol);
                return ol;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Remove;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }

        }

        public  Operationlogs Ops_ChangePersonState(Users u, long mappingID, Patrolpersonstates s)
        {
            try
            {
                //we have to check first that this person doesn't exists before in mapping
                var person_mapping_exists = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(e => e.Ahwalmappingid.Equals(mappingID));
                if (person_mapping_exists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ops_ChangePersonState;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على التوزيع";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //a bit different here, for Ahwalmapping, allowed states are only for sick,leave,sunrise,sunset
                int[] allowedState = {Core.Handler_AhwalMapping.PatrolPersonState_Sea,
                Core.Handler_AhwalMapping.PatrolPersonState_Land,
                Core.Handler_AhwalMapping.PatrolPersonState_Back,
                Core.Handler_AhwalMapping.PatrolPersonState_Away,
                Core.Handler_AhwalMapping.PatrolPersonState_WalkingPatrol,
                Core.Handler_AhwalMapping.PatrolPersonState_BackFromWalking};
                //first we have to check if this user is authorized to perform this transaction
                //if (!_user.isAuthorized(u.Userid, person_mapping_exists.Ahwalid, Handler_User.User_Role_Ops)
                //    && !allowedState.Contains(s.Patrolpersonstateid))
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ops_ChangePersonState;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}

                var GetPerson =_context.Persons.FirstOrDefault<Persons>(e => e.Personid.Equals(person_mapping_exists.Personid));
                if (GetPerson == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ops_ChangePersonState;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "لم يتم العثور على الفرد: " + person_mapping_exists.Personid; //todo, change it actual person name
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //for operations, we cannot change person state sea land or anythinf for associate
                if (person_mapping_exists.Patrolroleid == Core.Handler_AhwalMapping.PatrolRole_Associate)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ops_ChangePersonState;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                    ol_failed.Text = "المرافق لايمكن تغيير حالته: " + person_mapping_exists.Personid; //todo, change it actual person name
                                                                                                      // Handler_Operations.Add_New_Operation_Log(ol_failed); //no need to record this
                    return ol_failed;
                }
                person_mapping_exists.Patrolpersonstateid = s.Patrolpersonstateid;
                person_mapping_exists.Laststatechangetimestamp = DateTime.Now;
               _context.SaveChanges();
                //log it
                //record this in personstatechangelog
                var personStateLog = new Patrolpersonstatelog();
                personStateLog.Userid = u.Userid;
                personStateLog.Patrolpersonstateid = s.Patrolpersonstateid;
                personStateLog.Timestamp = DateTime.Now;
                personStateLog.Personid = person_mapping_exists.Personid;
                LogPersonStateChange(personStateLog);



                Operationlogs ol = new Operationlogs();
                ol.Userid = u.Userid;
                ol.Operationid = Handler_Operations.Opeartion_Mapping_Ops_ChangePersonState;
                ol.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol.Text = "عمليات تغيير حالة الفرد: " + GetPerson.Milnumber + " " + GetPerson.Name;
                _oper.Add_New_Operation_Log(ol);

                return ol;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Mapping_Ahwal_ChangePersonState;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
        }
    }
}
