using BlazorChatRoom.Shared;
using BlazorChatRoom.Shared.DTOs;
using BlazorChatRoom.Shared.DTOs.ChatDto;

namespace Application.Interfaces.Account;

public interface IUserService
{
    Task<ServiceResponse<long>> Register(RegisterDto request);
    Task<ServiceResponse<string>> Login(LoginDto request);
    Task<List<UserDto>> GetUsers(long userId);
    Task<long> GetUserIdByEmail(string email);
}