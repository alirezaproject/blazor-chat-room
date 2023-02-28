using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : BaseEntity<long>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public string Picture { get; set; } = string.Empty;
    public DateTime? LastOnline { get; set; }
    public bool IsOnline { get; set; }

}