using Microsoft.EntityFrameworkCore;

namespace criptoApiProyecto.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet <Cliente> Clientes { get; set; } 
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.Property(e => e.Money).HasColumnType("decimal(18,2)");

                entity.Property(e => e.CryptoAmount).HasColumnType("decimal(18.8)");
            });

        }
    }

   
}
