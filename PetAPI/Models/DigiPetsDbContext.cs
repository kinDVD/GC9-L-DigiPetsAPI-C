using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetAPI.Models;

public partial class DigiPetsDbContext : DbContext
{
    public DigiPetsDbContext()
    {
    }

    public DigiPetsDbContext(DbContextOptions<DigiPetsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DigiPet> DigiPets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=DigiPetsDB; Integrated Security=SSPI;Encrypt=false;TrustServerCertificate=True;");
            //=> optionsBuilder.UseSqlServer("Server=localhost,1433; Initial Catalog=DigiPetsDB; User ID=SA; Password=D3ffL6mI9frf; TrustServerCertificate=true;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DigiPet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DigiPets__3213E83F83204F8A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.Health)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("health");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Strength).HasColumnName("strength");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
