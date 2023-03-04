using BlazorChatRoom.Shared.DTOs.ChatDto;

namespace BlazorChatRoom.Client.Services.User;

public interface IUserService
{
    Task<List<UserDto>> GetUsers();
}