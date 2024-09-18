using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp
{
    
    public class ChatHub : Hub
    {
        public async Task  SendMessage(string name , string message)
        {
            await Clients.All.SendAsync("createmess",name, message);
        }
    }
}
