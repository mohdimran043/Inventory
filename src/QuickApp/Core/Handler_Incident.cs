using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MOI.Patrol;
using MOI.Patrol.DataAccessLayer;
namespace Core
{
    class Handler_Incidents
    {
        //states
        public const int Incident_State_New = 10;
        public const int Incident_State_HasComments = 20;
        public const int Incident_State_Closed = 30;
        private patrolsContext _context = new patrolsContext();
        private Handler_Operations _oper = new Handler_Operations();

        //all functions here require Ahwal Permssion
        public  Operationlogs Add_Incident(Users u, Incidents i)//this transaction requires User_Role_Ahwal Permisson on this AhwalID
        {
            try
            {
                //first we have to check if this user is authorized to perform this transaction
                Usersrolesmap permisson_esists = _context.Usersrolesmap.FirstOrDefault(r => r.Userid == u.Userid && r.Userroleid == Core.Handler_User.User_Role_Ops);

                if (permisson_esists == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_AddNew;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                i.Userid = u.Userid;
                i.Timestamp = DateTime.Now;
                i.Lastupdate = DateTime.Now;
                _context.Incidents.Add(i);
                //by default,we will add new comment for a new incidet,this will help later in the incidents dedicated page, that each incident at least has one comment

                _context.SaveChanges();
                //still, the incident status will be new, but with this we make sure that each incident at least has one comment
                var newIncidentComment = new Incidentscomments();
                newIncidentComment.Incidentid = i.Incidentid;
                newIncidentComment.Userid = u.Userid;
                newIncidentComment.Text = "قام باضافة البلاغ";
                newIncidentComment.Timestamp = DateTime.Now;
                _context.Incidentscomments.Add(newIncidentComment);
                _context.SaveChanges();
                //add it incidentview
                AddNewIncidentViewForAllExceptOriginalPoster(u, i);

                //create the opeartion log for it


            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_AddNew;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            Operationlogs ol = new Operationlogs();
            ol.Userid = u.Userid;
            ol.Operationid = Handler_Operations.Opeartion_Incidents_AddNew;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم اضافة البلاغ رقم: " + i.Incidentid.ToString();
            _oper.Add_New_Operation_Log(ol);
            return ol;
        }

        public Operationlogs HandOver_Incident_To_Person(Users u, Ahwalmapping m, Incidents i)
        {
            try
            {
                //first we have to check if this Users is authorized to perform this transaction
        
                //Usersrolesmap permisson_esists = _context.Usersrolesmap.FirstOrDefault(r => r.Userid == u.Userid && r.Userroleid == Core.Handler_User.User_Role_Ops);

                //if (permisson_esists == null)
                //{
                //    Operationlogs ol_failed = new Operationlogs();
                //    ol_failed.Userid = u.Userid;
                //    ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
                //    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                //    ol_failed.Text = "المستخدم لايملك صلاحية هذه العمليه";
                //    _oper.Add_New_Operation_Log(ol_failed);
                //    return ol_failed;
                //}

                var personmapping = _context.Ahwalmapping.FirstOrDefault<Ahwalmapping>(a => a.Ahwalmappingid == m.Ahwalmappingid);
                if (personmapping == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "لم يتم العثور على التوزيع رقم: " + m.Ahwalmappingid.ToString();
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }

                var Incidents = _context.Incidents.FirstOrDefault<Incidents>(a => a.Incidentid == i.Incidentid);
                if (Incidents == null)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "لم يتم العثور على البلاغ رقم: " + i.Incidentid.ToString();
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //we have to check the state of the person, we cannot hand over Incidents to wrong state
                if  (personmapping.Patrolpersonstateid != Core.Handler_AhwalMapping.PatrolPersonState_SunRise &&
                personmapping.Patrolpersonstateid != Core.Handler_AhwalMapping.PatrolPersonState_Sea &&
                personmapping.Patrolpersonstateid != Core.Handler_AhwalMapping.PatrolPersonState_Back &&
                personmapping.Patrolpersonstateid != Core.Handler_AhwalMapping.PatrolPersonState_WalkingPatrol &&
                personmapping.Patrolpersonstateid != Core.Handler_AhwalMapping.PatrolPersonState_BackFromWalking)
                {
                    Operationlogs ol_failed = new Operationlogs();
                    ol_failed.Userid = u.Userid;
                    ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
                    ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnAuthorized;
                    ol_failed.Text = "الدورية في حالة لاتسمح باستلام بلاغ" + i.Incidentid.ToString();
                    _oper.Add_New_Operation_Log(ol_failed);
                    return ol_failed;
                }
                //update mapping to show that person has an Incidents
                personmapping.Incidentid = Incidents.Incidentid;


                Incidents.Lastupdate = DateTime.Now;
                Incidents.Incidentstateid = Core.Handler_Incidents.Incident_State_HasComments;
                var newIncidentComment = new Incidentscomments();
                newIncidentComment.Incidentid = Incidents.Incidentid;
                newIncidentComment.Userid = u.Userid;
                newIncidentComment.Text = "قام بتسليم البلاغ للدورية صاحبة النداء: " + m.Callerid;
                newIncidentComment.Timestamp = DateTime.Now;
                _context.Incidentscomments.Add(newIncidentComment);
                _context.SaveChanges();
                AddNewIncidentViewForAllExceptOriginalPoster(u, i);

                Operationlogs ol_success = new Operationlogs();
                ol_success.Userid = u.Userid;
                ol_success.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
                ol_success.Statusid = Handler_Operations.Opeartion_Status_Success;
                ol_success.Text = "ناجح" + i.Incidentsourceid.ToString();
                _oper.Add_New_Operation_Log(ol_success);
                return ol_success;
            }
            catch (Exception ex)
            {
                Operationlogs ol_failed = new Operationlogs();
                ol_failed.Userid = u.Userid;
                ol_failed.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
                ol_failed.Statusid = Handler_Operations.Opeartion_Status_UnKnownError;
                ol_failed.Text = ex.Message;
                _oper.Add_New_Operation_Log(ol_failed);
                return ol_failed;
            }
            Operationlogs ol = new Operationlogs();
            ol.Userid = u.Userid;
            ol.Operationid = Handler_Operations.Opeartion_Incidents_HandOverIncident;
            ol.Statusid = Handler_Operations.Opeartion_Status_Success;
            ol.Text = "تم تسليم الدورية صاحبة النداء: " + m.Callerid.ToString() + "  البلاغ رقم: " + i.Incidentid.ToString();
            _oper.Add_New_Operation_Log(ol);
            return ol;
        }

        public  void AddNewIncidentViewForAllExceptOriginalPoster(Users ExceptUser, Incidents i)
        {
            List<Usersrolesmap> ul = _context.Usersrolesmap.Where(a => a.Userid != ExceptUser.Userid && a.Userroleid == Core.Handler_User.User_Role_Ops).Distinct().ToList<Usersrolesmap>();
            //the previous list will contain dublicates in case we have someone in more than one ahwal 
            //so we have to filter it again
            List<long> userIDs = new List<long>();
            foreach (var u in ul)
            {
                if (!userIDs.Contains(u.Userid))
                {
                    userIDs.Add(u.Userid);
                }
            }
            foreach (var u in userIDs)
            {
                try
                {
                    var existsBefore = _context.Incidentsview.FirstOrDefault(a => a.Userid == u && a.Incidentid == i.Incidentid);
                    if (existsBefore != null)
                    {//if exists just update timestamp
                        existsBefore.Timestamp = DateTime.Now;
                        // db2.IncidentsViews.DeleteOnSubmit(existsBefore);
                        _context.SaveChanges();
                    }
                    else
                    {//create new
                        var newIncidentView = new Incidentsview();
                        newIncidentView.Userid = u;
                        newIncidentView.Incidentid = i.Incidentid;
                        newIncidentView.Timestamp = DateTime.Now;
                        _context.Incidentsview.Add(newIncidentView); //I was ablt to use this new method because of this beautful entityextension.cs
                        _context.SaveChanges();
                    }

                }
                catch (Exception ex)
                {

                }
            }

        }

    }
}
