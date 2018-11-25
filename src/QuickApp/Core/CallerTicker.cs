using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MOI.Patrol.Core
{
    public class CallerTicker
    {
        private readonly SemaphoreSlim _marketStateLock = new SemaphoreSlim(1, 1);
        private IHubContext<CallerHub> Hub
        {
            get;
            set;
        }
        public CallerTicker(IHubContext<CallerHub> hub)
        {
            Hub = hub;          
        }
        public async Task GetCallerInfo()
        {
            await _marketStateLock.WaitAsync();
            try
            {
                CallerInfo m = new CallerInfo();
                m.CallerId = "12";
                m.OrgId = "12";
                m.Payload = "12";
                m.Type = "12";
                await BroadcastMarketStateChange(m);
            }
            finally
            {
                _marketStateLock.Release();
            }
        }
        
        private async Task BroadcastMarketStateChange(CallerInfo m)
        {
            await Hub.Clients.All.SendAsync("startcallevent", m);
        }

    }
}
