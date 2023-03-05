using BlazorChatRoom.Shared.DTOs.Message;

namespace Application.Interfaces.Message;

public interface IMessageService
{
    Task SaveChatMessage(MessageDto message);
    Task<List<MessageDto>> GetAllMessages();
}