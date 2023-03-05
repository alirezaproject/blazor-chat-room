using Domain.Contracts;
using Domain.Enums;

namespace Domain.Entities;

public class ChatMessage : BaseEntity<long>
{
    public long FromUserId { get; set; }
    public long ToUserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public virtual User FromUser { get; set; } = default!;
    public virtual User ToUser { get; set; } = default!;

}

