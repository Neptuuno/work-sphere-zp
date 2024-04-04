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
    private readonly UserService _userService;
    private static readonly Dictionary<string, string> UserConnectionMap = new Dictionary<string, string>();

    public ChatHub(ChatService chatService, UserService userService)
    {
        _chatService = chatService;
        _userService = userService;
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
        var senderUser = await _userService.GetSafeUserDetails(senderId);
        newMessage.Sender = senderUser;

        await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", newMessage);

        if (UserConnectionMap.TryGetValue(receiverId, out var receiverConnectionId))
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", newMessage);
        }
    }
    public async Task DeleteMessage(int messageId)
    {
        await _chatService.DeleteMessage(messageId);
        await Clients.All.SendAsync("MessageDeleted", messageId);
    }
}