using System.Security.AccessControl;
using Application.Interfaces.Context;
using Application.Interfaces.Message;
using BlazorChatRoom.Shared.DTOs.Chat;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Message;

public class MessageService : IMessageService
{
    private readonly IDataBaseContext _context;

    public MessageService(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task SaveChatMessage(MessageDto message)
    {
        message.ToUser = (await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync())!.Email;
       
        _context.ChatMessages.Add(new ChatMessage()
        {
            Message = message.Message,
            FromUserId = message.FromUserId,
            CreationDate = DateTime.Now,
            ToUserId = message.ToUserId,
           
            IsRemoved = false,
           // FromUser = (await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync())
        });
        await _context.SaveChangesAsync();
    }


    public async Task<List<MessageDto>> GetConversationAsync(string contactId, string userId)
    {
        return await _context.ChatMessages
            .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) ||
                        (h.FromUserId == userId && h.ToUserId == contactId))
            .OrderBy(a => a.CreationDate)
            .Include(a => a.FromUser)
            .Include(a => a.ToUser)
            .Select(x => new MessageDto()
            {
                FromUserId = x.FromUserId,
                Message = x.Message,
                CreatedDate = x.CreationDate,
                Id = x.Id,
                ToUserId = x.ToUserId,
                ToUser = x.ToUser.Email,
                FromUser = x.FromUser.Email
            }).ToListAsync();
    }
}