using Domain.Contracts;
using Domain.Enums;

namespace Domain.Entities;

public class ChatMessage : BaseEntity<long>
{
    public long SenderId { get; set; } = default!;
    public long ReceiverId { get; set; } = default!;
    public string Text { get; set; } = string.Empty;
    public MessageType MessageType { get; set; } = MessageType.Text;
    public MessageStatus MessageStatus { get; set; } = MessageStatus.Delivered;
    public DateTime Timestamp { get; set; } = DateTime.Now;


}

