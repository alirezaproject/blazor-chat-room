using System.Net.Http.Json;
using BlazorChatRoom.Shared;
using BlazorChatRoom.Shared.DTOs;

namespace BlazorChatRoom.Client.Services.User;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ServiceResponse<long>> Register(RegisterDto registerDto)
    {
        var result = await _http.PostAsJsonAsync("api/User/Register", registerDto);
        
        return (await result.Content.ReadFromJsonAsync<ServiceResponse<long>>())!;
    }
}