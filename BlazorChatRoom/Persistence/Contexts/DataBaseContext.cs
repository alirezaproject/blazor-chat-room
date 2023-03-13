using Application.Interfaces.Context;
using Domain.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Persistence.Contexts;

public class DataBaseContext : ApiAuthorizationDbContext<User>, IDataBaseContext
{
    public DataBaseContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOption) : base(options, operationalStoreOption)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(d => d.FromUser)
                .WithMany(p => p.ChatMessagesFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(d => d.ToUser)
                .WithMany(p => p.ChatMessagesToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
}