using BlazorChatRoom.Shared;
using BlazorChatRoom.Shared.DTOs;

namespace BlazorChatRoom.Client.Services.User;

public interface IAuthService
{
    event Action Onchange;
    Task<ServiceResponse<long>> Register(RegisterDto registerDto);
    Task<ServiceResponse<string>> Login(LoginDto loginDto);
}