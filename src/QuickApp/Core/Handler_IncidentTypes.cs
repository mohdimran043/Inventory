using System;
using System.Linq;
using MOI.Patrol;
namespace Core
{
    public class Handler_IncidentTypes
    {
        private patrolsContext _context = new patrolsContext();
        private Handler_Operations _oper = new Handler_Operations();

        public Operationlogs AddUpdate_IncidentType(int incidenttypeid, string incidenttypename, Users u)
        {
            try
            {
                //first we have to check if this user is authorized to perform this transaction
                Usersrolesmap permisson_esists = _context.Usersrolesmap.FirstOrDefault(r => r.Userid == u.Userid && r.Userroleid == Core.Handler_User.User_Role_Ops);
                if (permisson_esists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                if (incidenttypeid > 0)
                {
                    Incidentstypes it = _context.Incidentstypes.Where(i => i.Incidenttypeid == incidenttypeid).FirstOrDefault();
                    if (it == null)
                    {
                        Operationlogs ol_failed = new Operationlogs();
                        ol_failed.Userid = u.Userid;
                        ol_failed.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
                        ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                        ol_failed.Text = "Could not find the communication with the id";
                        _oper.Add_New_Operation_Log(ol_failed);
                        return ol_failed;
                    }
                    it.Name = incidenttypename;
                    _context.SaveChanges();
                }
                else
                {
                    Incidentstypes it = new Incidentstypes();
                    //CONFIRM WITH KHALIFA
                    it.Incidenttypeid = (_context.Incidentstypes.Count() + 1);
                    it.Name = incidenttypename;
                    _context.Incidentstypes.Add(it);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            Operationlogs ol = new Operationlogs();
            ol.Userid = u.Userid;
            ol.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم اضافة البلاغ رقم: " + incidenttypeid.ToString();
            _oper.Add_New_Operation_Log(ol);
            return ol;
        }

        public Operationlogs Delete_IncidentType(int incidenttypeid, Users u)
        {
            try
            {
                //first we have to check if this user is authorized to perform this transaction
                Usersrolesmap permisson_esists = _context.Usersrolesmap.FirstOrDefault(r => r.Userid == u.Userid && r.Userroleid == Core.Handler_User.User_Role_Ops);
                if (permisson_esists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                if (incidenttypeid > 0)
                {
                    Incidentstypes it = _context.Incidentstypes.Where(i => i.Incidenttypeid == incidenttypeid).FirstOrDefault();
                    if (it == null)
                    {
                        Operationlogs ol_failed = new Operationlogs();
                        ol_failed.Userid = u.Userid;
                        ol_failed.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
                        ol_failed.Statusid = Handler_Operations.Opeartion_Status_Failed;
                        ol_failed.Text = "Could not find the communication with the id";
                        _oper.Add_New_Operation_Log(ol_failed);
                        return ol_failed;
                    }
                    _context.Incidentstypes.Remove(it);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            Operationlogs ol = new Operationlogs();
            ol.Userid = u.Userid;
            ol.Operationid = Handler_Operations.Opeartion_IncidentsTypes_AddNew;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم اضافة البلاغ رقم: " + incidenttypeid.ToString();
            _oper.Add_New_Operation_Log(ol);
            return ol;

        }
    }
}
