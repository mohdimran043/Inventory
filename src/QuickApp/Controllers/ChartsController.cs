using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MOI.Patrol.DataAccessLayer;
using MOI.Patrol.Helpers;
using MOI.Patrol.ViewModels;
using Newtonsoft.Json;

namespace MOI.Patrol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : Controller
    {

        [HttpPost("getdeviceavailability")]
        public ActionResult GetDeviceAvailability([FromBody]  int ahwalsrcid)
        {
            ChartViewModel cvm = new ChartViewModel();

            return Ok(cvm);
        }
        [HttpPost("getemployeestats")]
        public ActionResult GetEmployeeStats([FromBody]  int ahwalId)
        {
            List<string> onDuty = new List<string>();
            List<string> onLeave = new List<string>();
            List<string> chartLabel = new List<string>();

            PostgresWrapper pw = new PostgresWrapper();
            Dictionary<string, object> keyValuePair = new Dictionary<string, object>();
            keyValuePair.Add("aid", ahwalId);
            DataSet pgDataSet = pw.ExecuteStoredProc(PatrolConstants.PGSQL_FETCHEMPLOYEESTATBYAHWAL, keyValuePair);
            if (pgDataSet != null && pgDataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in pgDataSet.Tables[0].Rows)
                {
                    chartLabel.Add(Convert.ToString(dr["name"]));
                    onDuty.Add(Convert.ToString(dr["ONDUTY"]));
                    onLeave.Add(Convert.ToString(dr["ONLEAVE"]));
                }
            }
            ChartViewModel cvm = new ChartViewModel();
            cvm.chartlabel = chartLabel.ToArray();
            cvm.chartsubdta = new List<ChartSubDataViewModel>();
            ChartSubDataViewModel csd = new ChartSubDataViewModel();
            csd.backgroundcolor = "";
            csd.data = onDuty.ToArray();
            csd.label = "On duty";
            cvm.chartsubdta.Add(csd);
            csd = new ChartSubDataViewModel();
            csd.backgroundcolor = "";
            csd.data = onLeave.ToArray();
            csd.label = "On leave";
            cvm.chartsubdta.Add(csd);
            return Ok(cvm);
        }
        [HttpPost("getincidentchart")]
        public ActionResult GetIncidentChart([FromBody]  int ahwalsrcid)
        {
            ChartViewModel cvm = new ChartViewModel();
            return Ok(cvm);
        }
        [HttpPost("getpatrolstatus")]
        public ActionResult GetPatrolStatus([FromBody]  int ahwalsrcid)
        {
            ChartViewModel cvm = new ChartViewModel();
            return Ok(cvm);
        }
    }
}