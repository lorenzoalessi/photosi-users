using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PhotosiUsers.Model;

[ExcludeFromCodeCoverage]
public class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}