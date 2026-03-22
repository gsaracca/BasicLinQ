using BasicLinq.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicLinq.Data;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");

            // Conversión bool? ↔ tinyint (NULL en BD → null en C#)
            entity.Property(e => e.IfCtaCte)
                  .HasColumnType("tinyint")
                  .HasConversion<byte?>(
                      v => v.HasValue ? (v.Value ? (byte)1 : (byte)0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            entity.Property(e => e.IfConsolidado)
                  .HasColumnType("tinyint")
                  .HasConversion<byte?>(
                      v => v.HasValue ? (v.Value ? (byte)1 : (byte)0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            entity.Property(e => e.IfVendedor)
                  .HasColumnType("tinyint")
                  .HasConversion<byte?>(
                      v => v.HasValue ? (v.Value ? (byte)1 : (byte)0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            entity.Property(e => e.IfTransporte)
                  .HasColumnType("tinyint")
                  .HasConversion<byte?>(
                      v => v.HasValue ? (v.Value ? (byte)1 : (byte)0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            // Conversión bool? ↔ int (NULL en BD → null en C#)
            entity.Property(e => e.IfNoFac)
                  .HasColumnType("int")
                  .HasConversion<int?>(
                      v => v.HasValue ? (v.Value ? 1 : 0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            entity.Property(e => e.IfCf)
                  .HasColumnType("int")
                  .HasConversion<int?>(
                      v => v.HasValue ? (v.Value ? 1 : 0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            entity.Property(e => e.IfDel)
                  .HasColumnType("int")
                  .HasConversion<int?>(
                      v => v.HasValue ? (v.Value ? 1 : 0) : null,
                      v => v.HasValue ? v.Value != 0 : null);

            entity.Property(e => e.IfSendMail)
                  .HasColumnType("int")
                  .HasConversion<int?>(
                      v => v.HasValue ? (v.Value ? 1 : 0) : null,
                      v => v.HasValue ? v.Value != 0 : null);
        });
    }
}
