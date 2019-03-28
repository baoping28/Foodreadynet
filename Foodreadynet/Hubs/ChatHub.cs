using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using FR.Infrastructure.Helpers;
using System.Threading.Tasks;
namespace Foodreadynet.Hubs
{
    public class ChatHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.User(CryptionClass.Decrypt(name)).addNewMessageToPage(name, message);
        }
        public void SendChatMessage(string name, string message)
        {
            // Call the MessageToPage method to update clients.
            Clients.All.ChatMessageToPage(name,message);
        }
        public void SendMessage(string groupid, string name, string itemid, string quantity, string productTitle,string itemTotal, string subtotal, string bizName, string tax, string driverTip, string globalSubtotal, string globalTotal, string btnShow)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid, Context.ConnectionId).MessageToPage(name, itemid, quantity, productTitle,itemTotal, subtotal, bizName, tax,driverTip, globalSubtotal, globalTotal, btnShow);
        }
        public void SendRemoveMessage(string groupid, string name, string itemid,string subtotal, string bizName, string tax,string driverTip, string globalSubtotal, string globalTotal, string btnShow)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid, Context.ConnectionId).RemoveToPage(name, itemid,subtotal, bizName, tax,driverTip, globalSubtotal, globalTotal, btnShow);
        }
        public void SendUpdatedMessage(string groupid, string name, string itemid, string quantity, string title, string itemSubtotal, string subtotal, string bizName, string tax, string driverTip, string globalSubtotal, string globalTotal, string btnShow)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid, Context.ConnectionId).UpdatedToPage(name, itemid, quantity, title,itemSubtotal, subtotal, bizName, tax,driverTip, globalSubtotal, globalTotal, btnShow);
        }
        public void SendDeliveryMessage(string groupid, string name, string isDelivery, string delFee, string driverTip, string globalTotal, string btnShow)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid, Context.ConnectionId).DeliveryToPage(name, isDelivery, delFee,driverTip, globalTotal, btnShow);
        }
        public void SendGroupMessage(string groupid, string name, string message)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid).GroupMessageToPage(name, message);
        }
        public void sendDoneMessage(string groupid, string name)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid).DoneMessageToPage(name);
        }
        public void sendKickMessage(string groupid, string name, string tax, string driverTip, string globalSubtotal, string globalTotal, string btnShow)
        {
            // Call the MessageToPage method to update clients.
            Clients.Group(groupid, Context.ConnectionId).KickToPage(name, tax, driverTip, globalSubtotal, globalTotal, btnShow);
        }
        public void SendOrder(string name, string biztitle,string orderid, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.User(name).SendNewOrder(biztitle,orderid, message);
        }
    }
}