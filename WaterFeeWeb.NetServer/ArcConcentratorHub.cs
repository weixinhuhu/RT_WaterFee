using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WaterFeeWeb.NetServer
{
    public class ArcConcentratorHub : Hub
    {
        public string getConnectionID()
        {
            return Context.ConnectionId;
        }

        public void SendParams(string json)
        { 
            //Clients.Client(Context.ConnectionId).SendMsg(json);
            Clients.All.keepAlive(json);
            //return "server:" + DateTime.Now.ToString();
        }

        public void SetParams(string fn, object jsonParams)
        {
            Clients.All.listenSetting(fn, jsonParams); 
        }

        public void CommandDownload(object json)
        {
            Clients.All.commandDownload(json);
        }

        public void CommandUpload(object json)
        {
            Clients.All.commandUpload(json);
        }

        public void SendMsg(string connectionID,string msg)
        {
            Clients.Client(connectionID).getMsg(msg);
        }
    }
}