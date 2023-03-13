
using BlazorChatRoom.Shared.DTOs;
using BlazorChatRoom.Shared.DTOs.Auth;
using BlazorChatRoom.Shared.DTOs.User;

namespace Application.Interfaces.Account;

public interface IUserService
{
    Task<ServiceResponse<string>> Register(RegisterDto request);
    Task<ServiceResponse<string>> Login(LoginDto request);
    Task<List<UserDto>> GetUsers(string userId);
    Task<UserDto> GetUserDetail(string userId);
    Task<string> GetUserIdByEmail(string email);
}
