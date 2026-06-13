using Microsoft.EntityFrameworkCore;

namespace PSS.amg538.Practica_04
{
    public class AutentificacionDbContext : DbContext
    {
        private readonly string _cs;
        public AutentificacionDbContext(string cs) => _cs = cs;

        public DbSet<UsuarioEntity> Usuarios => Set<UsuarioEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder ob) => ob.UseSqlServer(_cs);
        protected override void OnModelCreating(ModelBuilder mb)
            => mb.Entity<UsuarioEntity>().ToTable("Usuario");
    }
}
