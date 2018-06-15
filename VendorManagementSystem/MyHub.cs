using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace VendorManagementSystem
{
    public class MyHub : Hub
    {
        public void Announce(string message)
        {
            Clients.All.Announce(message);
        }

        public void BroadCastServerTime()
        {
            Clients.All.MessageReciever("Good Morning!!");
        }
    }
}