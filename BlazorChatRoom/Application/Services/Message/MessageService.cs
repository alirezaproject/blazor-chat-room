using System.Security.AccessControl;
using Application.Interfaces.Context;
using Application.Interfaces.Message;
using BlazorChatRoom.Shared.DTOs.Message;

namespace Application.Services.Message;

public class MessageService : IMessageService
{
    private readonly IDataBaseContext _context;

    public MessageService(IDataBaseContext context)
    {
        _context = context;
    }

    public  Task SaveChatMessage(MessageDto message)
    {
        throw new NotImplementedException();
    }

    public  Task<List<MessageDto>> GetAllMessages()
    {
        throw new NotImplementedException();
    }
}