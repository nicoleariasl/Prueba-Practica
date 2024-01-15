using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API.Models
{
    public partial class PruebaPContext : DbContext
    {
        public PruebaPContext()
        {
        }

        public PruebaPContext(DbContextOptions<PruebaPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArtVenta> ArtVenta { get; set; } = null!;
        public virtual DbSet<Detalle> Detalles { get; set; } = null!;
        public virtual DbSet<Encabezado> Encabezados { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=PML-209040662\\SQLEXPRESS; Database=PruebaP; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtVenta>(entity =>
            {
                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Iva).HasColumnName("IVA");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Detalle>(entity =>
            {
                entity.ToTable("Detalle");

                entity.Property(e => e.Total).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.CodArticuloNavigation)
                    .WithMany(p => p.Detalles)
                    .HasForeignKey(d => d.CodArticulo)
                    .HasConstraintName("FK_Detalle_Articulo");

                entity.HasOne(d => d.IdFacturaNavigation)
                    .WithMany(p => p.Detalles)
                    .HasForeignKey(d => d.IdFactura)
                    .HasConstraintName("FK_Detalle_Factura");
            });

            modelBuilder.Entity<Encabezado>(entity =>
            {
                entity.HasKey(e => e.FacturaId);

                entity.ToTable("Encabezado");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.Encabezados)
                    .HasForeignKey(d => d.Cliente)
                    .HasConstraintName("FK_Cliente_Factura");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Clave)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Usuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
