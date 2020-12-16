
using Argos.Domain;
using Argos.Domain.AlertaRoot;
using Argos.Domain.CameraLogRoot;
using Argos.Domain.CameraRoot;
using Argos.Domain.UsuarioPosicaoRoot;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Context
{
    public class Context : DbContext
    {
       // public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=argos;Uid=postgres;Pwd=root");
        }
        public DbSet<Alerta> DbSetAlerta { get; set; }
        public DbSet<Camera> DbSetCamera { get; set; }
        public DbSet<CameraLog> DbSetCameraLog { get; set; }
        public DbSet<UsuarioPosicao> DbSetUsuarioPosicao { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alerta>();
            modelBuilder.Entity<Camera>();
            modelBuilder.Entity<CameraLog>();
            modelBuilder.Entity<UsuarioPosicao>().ToTable("usuario_posicao");
        }
        
    }
}