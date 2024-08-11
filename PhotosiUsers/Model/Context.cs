﻿using Microsoft.EntityFrameworkCore;

namespace PhotosiUsers.Model;

public class Context : DbContext
{
    public Context()
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