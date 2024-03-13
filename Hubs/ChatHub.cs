using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Hubs;

public class ChatHub : Hub
{
    private readonly ChatService _chatService;
    private static readonly Dictionary<string, string> UserConnectionMap = new Dictionary<string, string>();

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

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

    public async Task SendMessage(int chatId, string senderId, string receiverId, string message)
    {
        var newMessage = await _chatService.CreateMessage(chatId, senderId, message);

        var senderConnectionId = UserConnectionMap[senderId];

        //TODO Send complex message instead of message content
        await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", newMessage.Content);

        if (UserConnectionMap.TryGetValue(receiverId, out var receiverConnectionId))
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", newMessage.Content);
        }
    }
}