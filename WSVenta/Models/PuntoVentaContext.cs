using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WSVenta.Models
{
    public partial class PuntoVentaContext : DbContext
    {
        public PuntoVentaContext()
        {
        }

        public PuntoVentaContext(DbContextOptions<PuntoVentaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemSale> ItemSales { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-90S6BF6;Database=PuntoVenta;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("cost");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("unitPrice");
            });

            modelBuilder.Entity<ItemSale>(entity =>
            {
                entity.ToTable("itemSale");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdItem).HasColumnName("idItem");

                entity.Property(e => e.IdSale).HasColumnName("idSale");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("unitPrice");

                entity.HasOne(d => d.IdItemNavigation)
                    .WithMany(p => p.ItemSales)
                    .HasForeignKey(d => d.IdItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_itemSale_item");

                entity.HasOne(d => d.IdSaleNavigation)
                    .WithMany(p => p.ItemSales)
                    .HasForeignKey(d => d.IdSale)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_itemSale_sale");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("sale");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("total");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("password")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
