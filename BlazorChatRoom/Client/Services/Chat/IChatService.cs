using BlazorChatRoom.Shared.DTOs.Chat;
using BlazorChatRoom.Shared.DTOs.User;
using Domain.Entities;

namespace BlazorChatRoom.Client.Services.Chat;

public interface IChatService
{
    Task<List<UserDto>?> GetUsersAsync();
    Task SaveMessageAsync(MessageDto message);
    Task<List<MessageDto>?> GetConversationAsync(string contactId);
    Task<UserDto> GetUserDetailsAsync(string userId);
}