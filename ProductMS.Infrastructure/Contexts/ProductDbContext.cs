using Microsoft.EntityFrameworkCore;
using ProductMS.Domain.Entities;

namespace ProductMS.Infrastructure.Contexts
{
    // Contexto de base de datos para productos
    public class ProductDbContext : DbContext
    {
        // Conjunto de datos para productos
        public DbSet<Product> Productos { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("productos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id"); // Mapea la propiedad 'Id' de C# a la columna 'id' de PostgreSQL

                entity.Property(e => e.SellerId).HasColumnName("usuario_id");
                entity.Property(e => e.Name).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("descripcion");
                entity.Property(e => e.Category).HasColumnName("categoria").HasMaxLength(50);
                entity.Property(e => e.BasePrice).HasColumnName("precio_base").HasPrecision(12, 2).IsRequired();
                entity.Property(e => e.Status).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("disponible");
                entity.Property(e => e.Images).HasColumnName("imagenes");
                entity.Property(e => e.CreatedAt).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}