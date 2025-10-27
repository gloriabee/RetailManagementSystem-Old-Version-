using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RetailManagementSystem.Models;

public partial class RetailDbContext : DbContext
{
    public RetailDbContext()
    {
    }

    public RetailDbContext(DbContextOptions<RetailDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-CKVVUFC\\MSSQLSERVER22;Database=RetailDB;User Id=sa;Password=admin123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07777EDF10");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("system");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC0746410B26");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("system");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__1EA48E88");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
