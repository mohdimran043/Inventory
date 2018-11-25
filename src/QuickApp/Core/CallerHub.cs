using Microsoft.AspNetCore.SignalR;
using MOI.Patrol.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOI.Patrol.Core
{
    public class CallerHub : Hub
    {
        //private CallerTicker _callerTicker;
        //public CallerHub(CallerTicker callerTicker)
        //{
        //    this._callerTicker = callerTicker;
        //}

        public async Task UpdateGrids(CallerInfo message)
        {
            await Clients.All.SendAsync("startcallevent", message);
        }
        public async Task GetCallerInfo()
        {
            CallerInfo m = new CallerInfo();
            m.CallerId = "12";
            m.OrgId = "1";
            m.Payload = "Caller 1 is taking to caller 2";
            m.Type = "Call";
            await Clients.All.SendAsync("startcallevent", m);
        }
        //private async Task BroadcastMarketStateChange(MarketState marketState)
        //{
        //    switch (marketState)
        //    {
        //        case MarketState.Open:
        //            await Hub.Clients.All.SendAsync("marketOpened");
        //            break;
        //        case MarketState.Closed:
        //            await Hub.Clients.All.SendAsync("marketClosed");
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
