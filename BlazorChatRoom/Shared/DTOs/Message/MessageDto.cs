namespace BlazorChatRoom.Shared.DTOs.Message;

public class MessageDto
{
    public string SenderName { get; set; } = string.Empty;
    public string SenderPicture { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Time { get; set; }




}