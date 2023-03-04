namespace BlazorChatRoom.Shared.DTOs.ChatDto;

public class UserDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PictureName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public bool IsOnline { get; set; } 
}