using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SocialNetwork.Hubs;

public class ChatHub : Hub
{
    // [Authorize]
    // public async Task SendMessage(string user, string receiverId, string message)
    // {
    //     await Clients.Client(receiverId).SendAsync("ReceiveMessage", user, message);
    // }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}