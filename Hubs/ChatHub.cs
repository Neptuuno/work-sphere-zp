using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SocialNetwork.Models;
using SocialNetwork.Models.InputModels;
using SocialNetwork.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SocialNetwork.Hubs;

public class ChatHub : Hub
{
    private readonly ChatService _chatService;
    private readonly UserService _userService;
    private readonly FileService _fileService;
    private static readonly Dictionary<string, string> UserConnectionMap = new Dictionary<string, string>();

    public ChatHub(ChatService chatService, UserService userService, FileService fileService)
    {
        _chatService = chatService;
        _userService = userService;
        _fileService = fileService;
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
            _fileService.DeleteFolder("message-images-temp", userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(int chatId, string senderId, string receiverId, MessageInputModel messageInput)
    {
        var newMessage = await _chatService.CreateMessage(chatId, senderId, messageInput);
        
        var senderConnectionId = UserConnectionMap[senderId];
        var senderUser = await _userService.GetSafeUserDetails(senderId);
        newMessage.Sender = senderUser;
        
        var newMessageJson = JsonSerializer.Serialize(newMessage, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        });
        
        await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", newMessageJson);

        if (UserConnectionMap.TryGetValue(receiverId, out var receiverConnectionId))
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", newMessageJson);
        }
    }
    public async Task DeleteMessage(int messageId)
    {
        await _chatService.DeleteMessage(messageId);
        await Clients.All.SendAsync("MessageDeleted", messageId);
    }
}