using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FinalProject.Hubs
{
    public class NotificationHub : Hub
    {
        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void LeaveGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
        }

        public void OnNewComment(string data, string groupName)
        {
            Clients.Group(groupName, Context.ConnectionId).addComment(data);
        }
    }
}