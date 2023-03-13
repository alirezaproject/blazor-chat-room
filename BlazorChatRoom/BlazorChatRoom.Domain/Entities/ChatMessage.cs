using Domain.Contracts;
using Domain.Enums;

namespace Domain.Entities;

public class ChatMessage : BaseEntity<long>
{
    public string FromUserId { get; set; } = string.Empty;
    public string ToUserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    public virtual User FromUser { get; set; } = default!;
    public virtual User ToUser { get; set; } = default!;

}

