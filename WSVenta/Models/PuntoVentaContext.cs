using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

#nullable disable

namespace WSVenta.Models
{
    public partial class PuntoVentaContext : DbContext
    {
        private readonly string _connectionString;

        public PuntoVentaContext()
        {

        }
        public PuntoVentaContext(DbContextOptions<PuntoVentaContext> options)
            : base(options)
        {
        }
        public PuntoVentaContext(IOptions<DbContextOptionsInfo> dbConnectionInfo)
        {
            _connectionString = dbConnectionInfo.Value.MyContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:pg-db-pv.database.windows.net,1433;Initial Catalog=my-sql-db2;Persist Security Info=False;User ID=pablo;Password=Ageofestacaroelhosting321g;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemSale> ItemSales { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cost>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IdItem })
                    .HasName("PK__cost__A8C27C1921C6E642");

                entity.ToTable("cost");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdItem).HasColumnName("idItem");

                entity.Property(e => e.Datechange)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("datechange");

                entity.Property(e => e.UnitCost)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("unitCost");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => new { e.IdCost, e.Id }, "IX_item_idCost_id");

                entity.HasIndex(e => new { e.IdPrice, e.Id }, "IX_item_idPrice_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdCost).HasColumnName("idCost");

                entity.Property(e => e.IdPrice).HasColumnName("idPrice");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => new { d.IdCost, d.Id })
                    .HasConstraintName("FK__item__02FC7413");

                entity.HasOne(d => d.Id1)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => new { d.IdPrice, d.Id })
                    .HasConstraintName("FK__item__03F0984C");
            });

            modelBuilder.Entity<ItemSale>(entity =>
            {
                entity.ToTable("itemSale");

                entity.HasIndex(e => e.IdItem, "IX_itemSale_idItem");

                entity.HasIndex(e => e.IdSale, "IX_itemSale_idSale");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdItem).HasColumnName("idItem");

                entity.Property(e => e.IdSale).HasColumnName("idSale");

                entity.Property(e => e.Profit)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("profit");

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
                    .HasConstraintName("FK_itemSale_sale");
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IdItem })
                    .HasName("PK__price__A8C27C19CEF57807");

                entity.ToTable("price");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdItem).HasColumnName("idItem");

                entity.Property(e => e.Datechange)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("datechange");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("unitPrice");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("sale");

                entity.HasIndex(e => e.IdUser, "IX_sale_idUser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("total");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_sale_user");
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
