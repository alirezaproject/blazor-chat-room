using BlazorChatRoom.Shared.DTOs.Chat;

namespace Application.Interfaces.Message;

public interface IMessageService
{
    Task SaveChatMessage(MessageDto message);
    Task<List<MessageDto>> GetConversationAsync(string contactId, string userId);
}