using ApiMunicipio.Controllers;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Data
{
    public class MunicipioContext : IdentityDbContext<Usuario>
    {

        public MunicipioContext(DbContextOptions<MunicipioContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Taller>().HasData(
                new Taller
                {
                    IdTaller = 1,
                    NombreTaller = "Taller de Cocina",
                    DescripcionTaller = "Capacitación comunitaria",
                    FechaInicio = new DateOnly(2026, 6, 1),
                    FechaFin = new DateOnly(2026, 7, 30),
                    HoraInicio = new TimeOnly(18, 0),
                    HoraFin = new TimeOnly(20, 0),
                    SectorTaller = "Quitumbe",
                    CuposTaller = 40
                },
                new Taller
                {
                    IdTaller = 2,
                    NombreTaller = "Taller de Baile",
                    DescripcionTaller = "Capacitación comunitaria",
                    FechaInicio = new DateOnly(2026, 7, 1),
                    FechaFin = new DateOnly(2026, 12, 30),
                    HoraInicio = new TimeOnly(10, 0),
                    HoraFin = new TimeOnly(12, 0),
                    SectorTaller = "Solanda",
                    CuposTaller = 40
                }
            );

        }

        //public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Taller> Talleres { get; set; }
        public DbSet<Feria> Ferias { get; set; }
        //public DbSet<Inscripcion> Inscripciones { get; set; }
        //public DbSet<Animal> Animales { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        //public DbSet<Alerta> Alertas { get; set; }
        //public DbSet<CasoSocial> CasosSociales { get; set; }
        //public DbSet<Violencia> Violencias { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
       // public DbSet<Reserva> Reservas { get; set; }
       // public DbSet<Permiso> Permisos { get; set; }
        //public DbSet<Obligacion> Obligaciones { get; set; }



    }
}
