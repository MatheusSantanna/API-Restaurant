using Microsoft.EntityFrameworkCore;
using backend.Model;
using Microsoft.EntityFrameworkCore.Internal;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Mesas> Mesas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ItensPedido> ItensPedidos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<Produtos> Produtos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);


        }
  
    }
}
