using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Context;

public interface IDataBaseContext
{
    DbSet<User> Users { get; set; }
    DbSet<ChatMessage> ChatMessages { get; set; }

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new());

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}