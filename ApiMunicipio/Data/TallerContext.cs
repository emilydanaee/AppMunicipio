using ApiMunicipio.Controllers;
using ApiMunicipio.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Data
{
    public class MunicipioContext : DbContext
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
        public DbSet<Taller> Talleres { get; set; }
    }
}
