using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public DateTime? LastOnline { get; set; }
    public bool IsRemoved { get; set; } = false;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? ModifiedDate { get; set; }
    public DateTime? RemoveDate { get; set; }


    public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; } = default!;
    public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; } = default!;

    public User()
    {
        ChatMessagesFromUsers = new HashSet<ChatMessage>();
        ChatMessagesToUsers = new HashSet<ChatMessage>();
    }
}