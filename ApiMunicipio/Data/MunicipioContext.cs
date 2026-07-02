using ApiMunicipio.Controllers;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Data
{
    public class MunicipioContext : IdentityDbContext<Usuario>
    {

        public MunicipioContext(DbContextOptions<MunicipioContext> options) : base(options) { }


        // MODULO 1:
        public DbSet<Taller> Talleres { get; set; }
        public DbSet<InscripcionTaller> InscripcionesTaller { get; set; }
        public DbSet<Feria> Ferias { get; set; }
        public DbSet<Campania> Campanias { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        // MODULO 2:
        public DbSet<ReporteDTO> Reportes { get; set; }
        public DbSet<Alerta> Alertas{ get; set; }
        public DbSet<ReporteViolencia> ReportesViolencia { get; set; }
        public DbSet<SituacionCalle> SituacionCalle { get; set; }
        
        // MODULO 3:
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Obra> Obras { get; set; }



    }
}
