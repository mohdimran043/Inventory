using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MOI.Patrol.Core;
using MOI.Patrol.Interface;
using System;

namespace MOI.AssetManagement.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        //---------------------------------------------------------------------------
        //In POSTMAN CALL THE FOLLOWING
        //http://localhost:2021/api/Message/StartCall
        //http://localhost:2021/api/Message/StopCall
        //---------------------------------------------------------------------------
        private IHubContext<CallerHub> _callerHub
        {
            get;
            set;
        }
        public MessageController(IHubContext<CallerHub> callerhub)
        {
            _callerHub = callerhub;
        }
        [HttpGet("StartCall")]
        public string StartCall()
        {
            Random rnd = new Random();
            int id = rnd.Next(1, 13); 
            int caller = rnd.Next(65);
            int caller2 = rnd.Next(54);
            CallerInfo m = new CallerInfo();
            m.CallerId = id.ToString();
            m.OrgId = "1";
            m.Payload = "Caller "+ caller+" is taking to "+caller2;
            m.Type = "Call";
            _callerHub.Clients.All.SendAsync("startcallevent", m);
            return "";

        }
        [HttpGet("StopCall")]
        public string StopCall()
        {
            Random rnd = new Random();
            int id = rnd.Next(1, 13);
            int caller = rnd.Next(65);
            int caller2 = rnd.Next(54);
            CallerInfo m = new CallerInfo();
            m.CallerId = id.ToString();
            m.OrgId = "1";
            m.Payload = "Caller " + caller + " is taking to " + caller2;
            m.Type = "Call";
            _callerHub.Clients.All.SendAsync("endcallevent", m);
            return "";

        }
        //private IHubContext<CallerHub, ITypedHubClient> _hubContext;

        //public MessageController(IHubContext<CallerHub, ITypedHubClient> hubContext)
        //{
        //    _hubContext = hubContext;
        //}

        //[HttpPost]
        //public string Post([FromBody]Message msg)
        //{
        //    string retMessage = string.Empty;

        //    try
        //    {
        //        _hubContext.Clients.All.BroadcastMessage(msg.Type, msg.Payload, msg.OrgId, msg.CallerId);
        //        retMessage = "Success";
        //    }
        //    catch (Exception e)
        //    {
        //        retMessage = e.ToString();
        //    }

        //    return retMessage;
        //}
    }
}