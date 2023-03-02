using System.Security.Claims;
using Application.Interfaces.Context;
using BlazorChatRoom.Shared.DTOs.ChatDto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatRoom.Server.Hubs;

public class ChatHub : Hub
{
    private readonly IDataBaseContext _context;
    private static readonly List<UserConnection> Connections = new();

    public ChatHub(IDataBaseContext context)
    {
        _context = context;
     
    }


    public async Task SendMessage(long senderId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", senderId.ToString(), message);

    }

    public async Task<List<long>> GetOnlineUsers()
    {
        var users = Connections.Select(c => c.UserId).ToList();
        return await Task.FromResult(users);
    }

    public override Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();

        var email = context!.Request.Query["username"].ToString();
        var connectionId = Context.ConnectionId;
       
        Connections.Add(new UserConnection()
        {
            ConnectionId = connectionId,
            UserId =_context.Users.First(x => x.Email == email).Id
        });

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connection = Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
        if (connection != null)
        {
            Connections.Remove(connection);
        }
        return base.OnDisconnectedAsync(exception);
    }
}