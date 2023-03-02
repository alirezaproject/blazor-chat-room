namespace BlazorChatRoom.Shared.DTOs.ChatDto;

public class UserConnection
{
    public string ConnectionId { get; set; } = string.Empty;
    public long UserId { get; set; } = default!;
}