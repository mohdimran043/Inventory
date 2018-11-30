using MOI.Patrol.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOI.Patrol.DataAccessLayer
{
    public static class PatrolUserManager
    {
        private static patrolsContext _context = new patrolsContext();
        public static Users GetUserByUserName(string userName)
        {
            Users u = null;
            if (!string.IsNullOrEmpty(userName))
            {
                u = _context.Users.FirstOrDefault(ur => ur.Username == userName);
            }

            return u;
        }

        public static List<string> GetRolesByUserId(long userid)
        {
            List<string> roles = new List<String>();
            if (userid > 0)
            {

                roles = (from r in _context.Usersroles
                         join rm in _context.Usersrolesmap
                         on r.Userroleid equals rm.Userroleid
                         where rm.Userid == userid
                         select r.Name).ToList();

            }

            return roles.ToList();
        }

        public static string FetchLeftNavigationByUserId(long userid)
        {

            List<string> roles = GetRolesByUserId(userid);
            List<LeftNavigation> lnLst = new List<LeftNavigation>();
            foreach (var r in roles)
            {
                LeftNavigation ln = FetchLeftNavigatioEntity(r);
                if (!string.IsNullOrEmpty(ln.label))
                {
                    lnLst.Add(ln);
                }
            }

            return JsonConvert.SerializeObject(lnLst.OrderBy(l => l.order));
        }
        private static LeftNavigation FetchLeftNavigatioEntity(string role)
        {
            LeftNavigation ln = new LeftNavigation();
            switch (role)
            {
                case "ManageCharts":
                    ln.label = "الإحصاء";
                    ln.route = "Home";
                    ln.iconClasses = "fa fa-pie-chart";
                    ln.order = 1;
                    break;
                case "ManageOrganization":

                    break;
                case "ManageOperation":
                    ln.label = "العمليات";
                    ln.route = "operations/operationsopslive";
                    ln.iconClasses = "fa fa-user-secret";
                    ln.order = 4;
                    LeftNavigation child1 = new LeftNavigation();
                    child1.label = "الكشف";
                    child1.route = "operations/operationsopslive";
                    child1.iconClasses = "fa fa-user-secret";
                    LeftNavigation child2 = new LeftNavigation();
                    child2.label = "البلاغات";
                    child2.route = "operations/incidents";
                    child2.iconClasses = "fa  fa-file-text-o";
                    LeftNavigation child3 = new LeftNavigation();
                    child3.label = "نوع الحادث";
                    child3.route = "operations/incidenttype";
                    child3.iconClasses = "fa fa-file-o";

                   

                    ln.children = new List<LeftNavigation>();
                    ln.children.Add(child1);
                    ln.children.Add(child2);
                    ln.children.Add(child3);


                    break;
                case "ManageDispatcher":
                    ln.label = "الأحوال";
                    ln.route = "dispatcher/dispatcher";
                    ln.iconClasses = "fa fa-industry";
                    ln.order = 3;
                    LeftNavigation child4 = new LeftNavigation();
                    child4.label = "كشف التوزيع";
                    child4.route = "dispatcher/dispatcher";
                    child4.iconClasses = "fa fa-industry";

                    ln.children = new List<LeftNavigation>();
                    ln.children.Add(child4);
                    break;
                case "ManageMaintainance":
                    ln.label = "الصيانه";
                    ln.route = "maintainence/patrolcars";
                    ln.iconClasses = "fa fa-th-list";
                    ln.order = 2;
                    LeftNavigation child5 = new LeftNavigation();
                    child5.label = "الصيانه";
                    child5.route = "maintainence/patrolcars";
                    child5.iconClasses = "fa fa-automobile";
                    LeftNavigation child6 = new LeftNavigation();
                    child6.label = "تقارير الاستلام والتسليم الأجهزة";
                    child6.route = "maintainence/patrolcarsinventory";
                    child6.iconClasses = "fa fa-calendar";
                    LeftNavigation child7 = new LeftNavigation();
                    child7.label = "الأجهزة";
                    child7.route = "maintainence/handhelds";
                    child7.iconClasses = "fa fa-fax";
                    LeftNavigation child8 = new LeftNavigation();
                    child8.label = "تقارير الاستلام والتسليم الدوريات";
                    child8.route = "maintainence/handheldsinventory";
                    child8.iconClasses = "fa fa-calendar";

                    ln.children = new List<LeftNavigation>();
                    ln.children.Add(child5);
                    ln.children.Add(child6);
                    ln.children.Add(child7);
                    ln.children.Add(child8);
                    break;
                case "ManageScheduling":
                    break;
                default:
                    break;
            }

            return ln;
        }
    }
}
