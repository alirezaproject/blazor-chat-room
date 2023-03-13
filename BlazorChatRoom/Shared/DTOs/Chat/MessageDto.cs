

namespace BlazorChatRoom.Shared.DTOs.Chat;

public class MessageDto
{
    public string FromUserId { get; set; } = string.Empty;
    public string ToUserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public string FromUser { get; set; } = string.Empty;
    public string ToUser { get; set; } = string.Empty;
    public long Id { get; set; }
}