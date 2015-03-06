using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Automato.Web.Hubs
{
    public class DeviceStateHub : Hub
    {
        public void BroadcastStateUpdate(string deviceInternalName, string state)
        {
            Clients.All.broadcastStateUpdate(new
            {
                deviceInternalName = deviceInternalName,
                state = state
            });
        }
    }
}