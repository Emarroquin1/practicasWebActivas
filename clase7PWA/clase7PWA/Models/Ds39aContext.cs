using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace clase7PWA.Models;

public partial class Ds39aContext : DbContext
{
    public Ds39aContext()
    {
    }

    public Ds39aContext(DbContextOptions<Ds39aContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cuenta> Cuentas { get; set; }

    public virtual DbSet<DetalleCuenta> DetalleCuentas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-KG7C6J2\\SQLEXPRESS;DataBase=ds39a; Trusted_Connection=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.NumeroCuenta).HasName("PK__cuentas__E039507A5F066067");

            entity.ToTable("cuentas");

            entity.Property(e => e.NumeroCuenta).ValueGeneratedNever();
            entity.Property(e => e.Saldo).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<DetalleCuenta>(entity =>
        {
            entity.HasKey(e => e.Iddetalle).HasName("PK__Detalle___6FE8F71FED087200");

            entity.ToTable("Detalle_Cuentas");

            entity.Property(e => e.Iddetalle)
                .ValueGeneratedNever()
                .HasColumnName("iddetalle");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.NumeroCuentaNavigation).WithMany(p => p.DetalleCuenta)
                .HasForeignKey(d => d.NumeroCuenta)
                .HasConstraintName("FK__Detalle_C__Numer__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
