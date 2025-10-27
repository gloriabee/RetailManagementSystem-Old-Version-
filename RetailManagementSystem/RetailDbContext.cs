using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RetailManagementSystem.Models;

namespace RetailManagementSystem;

public partial class RetailDbContext : DbContext
{
    public RetailDbContext()
    {
    }

    public RetailDbContext(DbContextOptions<RetailDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("E:\\Internship\\WPF\\RetailManagementSystem\\RetailManagementSystem\\appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("RetailDB");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0724DA5823");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534547BA59F").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("system");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("sysutcdatetime()");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07174A40DC");

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("sysutcdatetime()");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("system");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("sysutcdatetime()");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
