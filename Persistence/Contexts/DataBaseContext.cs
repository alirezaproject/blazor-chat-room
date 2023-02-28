using Application.Interfaces.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class DataBaseContext : DbContext,IDataBaseContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
        
    }


    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
}