using Domain.Entities;
using System.Net.Http.Json;
using BlazorChatRoom.Shared.DTOs.Chat;
using BlazorChatRoom.Shared.DTOs.User;

namespace BlazorChatRoom.Client.Services.Chat;

public class ChatService : IChatService
{
    private readonly HttpClient _httpClient;
    public ChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<MessageDto>?> GetConversationAsync(string contactId)
    {
        return (await _httpClient.GetFromJsonAsync<List<MessageDto>>($"api/chat/{contactId}"))!;
    }
    public async Task<UserDto> GetUserDetailsAsync(string userId)
    {
        return (await _httpClient.GetFromJsonAsync<UserDto>($"api/chat/users/{userId}"))!;
    }
    public async Task<List<UserDto>?> GetUsersAsync()
    {
        var data = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/Chat/users");
        return data;
    }
    public async Task SaveMessageAsync(MessageDto message)
    {
        await _httpClient.PostAsJsonAsync("api/Chat/SaveMessage", message);
    }
}