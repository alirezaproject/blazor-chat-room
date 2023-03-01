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

    public event Action? Onchange;

    public async Task<ServiceResponse<long>> Register(RegisterDto registerDto)
    {
        var result = await _http.PostAsJsonAsync("api/User/Register", registerDto);
       
        return (await result.Content.ReadFromJsonAsync<ServiceResponse<long>>())!;
    }

    public async Task<ServiceResponse<string>> Login(LoginDto loginDto)
    {
        var result = await _http.PostAsJsonAsync("api/User/Login", loginDto);
     
        return (await result.Content.ReadFromJsonAsync<ServiceResponse<string>>())!;
    }
}