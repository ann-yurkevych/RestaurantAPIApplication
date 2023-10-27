using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPIApplication.Models;

public partial class RestaurantDbContext : DbContext
{
    public RestaurantDbContext()
    {
    }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-3H538OJ\\SQLSERVER;Database=RestaurantDB; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Favourites_Clients");

            entity.HasOne(d => d.Place).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favourites_Places");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.Property(e => e.CloseTime)
                .IsRequired()
                .HasMaxLength(8);
            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.OpenTime)
                .IsRequired()
                .HasMaxLength(8);

            entity.HasOne(d => d.Type).WithMany(p => p.Places)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Places_Types");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Ratings_Clients");

            entity.HasOne(d => d.Place).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratings_Places");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Type");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
