using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KanabanBack.Models;

public partial class NewkanbanContext : DbContext
{
    public NewkanbanContext()
    {
    }

    public NewkanbanContext(DbContextOptions<NewkanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<HistorialRefreshToken> HistorialRefreshTokens { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.BoardId).HasName("PK__Board__F9646BF289F79176");

            entity.ToTable("Board");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(255);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Boards)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Board__UsuarioId__4CA06362");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Card__55FECDAE5B154417");

            entity.ToTable("Card");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.List).WithMany(p => p.Cards)
                .HasForeignKey(d => d.ListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Card__ListId__5441852A");

            entity.HasOne(d => d.Tag).WithMany(p => p.Cards)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__Card__TagId__5629CD9C");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Cards)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Card__UsuarioId__5535A963");
        });

        modelBuilder.Entity<HistorialRefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdHistorialToken).HasName("PK__Historia__03DC48A576411FB9");

            entity.ToTable("HistorialRefreshToken");

            entity.Property(e => e.EsActivo).HasComputedColumnSql("(case when [FechaExpiracion]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialRefreshTokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Historial__IdUsu__59063A47");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.ListId).HasName("PK__List__E383280505E0F1B1");

            entity.ToTable("List");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(255);

            entity.HasOne(d => d.Board).WithMany(p => p.Lists)
                .HasForeignKey(d => d.BoardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__List__BoardId__4F7CD00D");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tags__3213E83F1A446FD7");

            entity.ToTable("tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078551D307");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Users__6B0F5AE0DF0C752A").IsUnique();

            entity.Property(e => e.Clave)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
