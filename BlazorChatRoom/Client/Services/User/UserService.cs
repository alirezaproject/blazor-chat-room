using System.Net.Http.Json;
using BlazorChatRoom.Shared.DTOs.ChatDto;

namespace BlazorChatRoom.Client.Services.User;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var result = await _httpClient.GetAsync("api/User/users");
        return result.Content.ReadFromJsonAsync<List<UserDto>>().Result!;
    }
}