using BlazorChatRoom.Shared;
using BlazorChatRoom.Shared.DTOs;

namespace Application.Interfaces.Account;

public interface IUserService
{
    Task<ServiceResponse<long>> Register(RegisterDto request);
}