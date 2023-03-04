using System.Security.Claims;
using Application.Interfaces.Account;
using Application.Interfaces.Context;
using BlazorChatRoom.Shared.DTOs.ChatDto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatRoom.Server.Hubs;

public class ChatHub : Hub
{
    private readonly IDataBaseContext _context;
    private static readonly List<UserConnection> Connections = new();
    private readonly IUserService _userService;
    
    public ChatHub(IDataBaseContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
       
    }


    public async Task SendMessage(long senderId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", senderId.ToString(), message);

    }

    public async Task<List<UserDto>> GetUsers(string  username)
    {
        var userId =await _userService.GetUserIdByEmail(username);
        var users = await _userService.GetUsers(userId);
        var onlineUsersId = Connections.Select(c => c.UserId).ToList();

        foreach (var user in users)
        {
            if (onlineUsersId.Contains(user.Id))
            {
                user.IsOnline = true;
            }
        }

        return await Task.FromResult(users);
    }

    public override async Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();

        var email = context!.Request.Query["username"].ToString();
        var connectionId = Context.ConnectionId;
        var userId = _context.Users.First(x => x.Email == email).Id;
        Connections.Add(new UserConnection()
        {
            ConnectionId = connectionId,
            UserId =userId
        });
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId, userId);
        
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connection = Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
        if (connection != null)
        {
            Connections.Remove(connection);
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId, connection.UserId);
        }
     
    }
}