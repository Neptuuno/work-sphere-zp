using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Models;

namespace SocialNetwork.Hubs;

public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> UserConnectionMap = new Dictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {
            UserConnectionMap[userId] = Context.ConnectionId;
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {
            UserConnectionMap.Remove(userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    // public async Task SendMessage(string senderId, string receiverId, string message)
    // {
    //     // if (UserConnectionMap.TryGetValue(receiverId, out var connectionId))
    //     // {
    //     //     // await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
    //     //     await Clients.All.SendAsync("ReceiveMessage", senderId, message);
    //     // }
    // }
    public async Task SendMessage(string senderId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", senderId, message);
    }
}