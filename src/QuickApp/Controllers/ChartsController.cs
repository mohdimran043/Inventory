using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MOI.Patrol.ViewModels;
using Newtonsoft.Json;

namespace MOI.Patrol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : Controller
    {

        [HttpPost("getdeviceavailability")]
        public ActionResult GetDeviceAvailability(int ahwalsrcid)
        {
            ChartViewModel cvm = new ChartViewModel();

            return Ok(cvm);
        }
        [HttpPost("getemployeestats")]
        public ActionResult GetEmployeeStats(int ahwalsrcid)
        {
            List<string> ds = new List<string> { "120", "110", "340" };
            List<string> dsLeave = new List<string> { "12", "11", "34" };
            List<string> lsLabel = new List<string> { "custom section1", "custom section2", "custom section3" };
            ChartViewModel cvm = new ChartViewModel();
            cvm.chartlabel = lsLabel.ToArray();
            cvm.chartsubdta = new List<ChartSubDataViewModel>();
            ChartSubDataViewModel csd = new ChartSubDataViewModel();
            csd.backgroundcolor = "";
            csd.data= ds.ToArray();
            csd.label = "On duty";
            cvm.chartsubdta.Add(csd);
            csd = new ChartSubDataViewModel();
            csd.backgroundcolor = "";
            csd.data = dsLeave.ToArray();
            csd.label = "On leave";
            cvm.chartsubdta.Add(csd);
            return Ok(cvm);
        }
        [HttpPost("getincidentchart")]
        public ActionResult GetIncidentChart(int ahwalsrcid)
        {
            ChartViewModel cvm = new ChartViewModel();
            return Ok(cvm);
        }
        [HttpPost("getpatrolstatus")]
        public ActionResult GetPatrolStatus(int ahwalsrcid)
        {
            ChartViewModel cvm = new ChartViewModel();
            return Ok(cvm);
        }
    }
}