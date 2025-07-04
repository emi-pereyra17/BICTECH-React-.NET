using Microsoft.EntityFrameworkCore;
using BicTechBack.src.Core.Entities;

namespace BicTechBack.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidosDetalles { get; set; }
        public DbSet<CarritoDetalle> CarritosDetalles { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<CategoriaMarca> CategoriasMarcas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PedidoDetalle>()
                .Property(pd => pd.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PedidoDetalle>()
                .Property(pd => pd.Subtotal)
                .HasColumnType("decimal(18,2)");
        }
    }
}
