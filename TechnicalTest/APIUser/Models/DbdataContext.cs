using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIUser.Models;

public partial class dbdataContext : DbContext
{
    public dbdataContext()
    {
    }

    public dbdataContext(DbContextOptions<dbdataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRol> UserRols { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:dbdata");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.idUser);

            entity.ToTable("User");

            entity.Property(e => e.address)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.birthday).HasColumnType("datetime");
            entity.Property(e => e.email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.registered)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.updated).HasColumnType("datetime");

            entity.HasOne(d => d.idUserRolNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.idUserRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserRol");
        });

        modelBuilder.Entity<UserRol>(entity =>
        {
            entity.HasKey(e => e.idUserRole);

            entity.ToTable("UserRol");

            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.registered)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.updated).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
