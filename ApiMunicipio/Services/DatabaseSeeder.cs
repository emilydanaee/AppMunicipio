using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Services;

public static class DatabaseSeeder
{
    public const string AdminEmail = "admin@quito.gob.ec";
    public const string AdminPassword = "Admin123";
    public const string CitizenEmail = "ciudadano@quito.gob.ec";
    public const string CitizenPassword = "Ciudadano123";
    public const string CitizenCedula = "1712345678";

    public static async Task InitializeAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<MunicipioContext>();
        await context.Database.MigrateAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "Usuario" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        await EnsureUsersAsync(services);

        var today = DateOnly.FromDateTime(DateTime.Today);

        if (!await context.Talleres.AnyAsync())
        {
            context.Talleres.AddRange(
                new Taller
                {
                    NombreTaller = "Taller de Reciclaje y Compostaje",
                    DescripcionTaller = "Aprende a separar residuos y crear compost para tu hogar.",
                    FechaInicio = today.AddDays(7),
                    FechaFin = today.AddDays(7),
                    HoraInicio = new TimeOnly(10, 0),
                    HoraFin = new TimeOnly(12, 0),
                    SectorTaller = "Sur",
                    UbicacionTaller = "Casa Somos Quitumbe",
                    Instructor = "Equipo ambiental municipal",
                    Dias = "Sábado",
                    Modalidad = "Presencial",
                    CuposTaller = 40
                },
                new Taller
                {
                    NombreTaller = "Formación en Derechos Ciudadanos",
                    DescripcionTaller = "Conoce tus derechos y los canales de atención del Municipio.",
                    FechaInicio = today.AddDays(12),
                    FechaFin = today.AddDays(12),
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(12, 0),
                    SectorTaller = "Centro",
                    UbicacionTaller = "Centro de Desarrollo Comunitario",
                    Instructor = "Participación Ciudadana",
                    Dias = "Jueves",
                    Modalidad = "Presencial",
                    CuposTaller = 25
                });
        }

        if (!await context.Ferias.AnyAsync())
        {
            context.Ferias.AddRange(
                new Feria
                {
                    NombreFeria = "Feria de Emprendimientos Locales",
                    DescripcionFeria = "Productos, alimentos y artesanías de emprendedores quiteños.",
                    FechaInicio = today.AddDays(9),
                    FechaFin = today.AddDays(9),
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(16, 0),
                    SectorFeria = "Norte",
                    UbicacionFeria = "Parque Bicentenario",
                    ImagenFeria = string.Empty
                },
                new Feria
                {
                    NombreFeria = "Feria de Salud Comunitaria",
                    DescripcionFeria = "Servicios preventivos y orientación gratuita para la comunidad.",
                    FechaInicio = today.AddDays(16),
                    FechaFin = today.AddDays(16),
                    HoraInicio = new TimeOnly(8, 30),
                    HoraFin = new TimeOnly(14, 0),
                    SectorFeria = "Centro",
                    UbicacionFeria = "Plaza de San Francisco",
                    ImagenFeria = string.Empty
                });
        }

        if (!await context.Campanias.AnyAsync())
        {
            context.Campanias.AddRange(
                new Campania
                {
                    NombreCampania = "Jornada de Bienestar Animal",
                    TipoCampania = "Atención veterinaria",
                    DescripcionCampania = "Chequeo básico, orientación y desparasitación para animales de compañía.",
                    FechaInicio = today.AddDays(10),
                    FechaFin = today.AddDays(10),
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(14, 0),
                    UbicacionCampania = "Parque La Carolina",
                    ResponsableCampania = "Unidad de Bienestar Animal"
                },
                new Campania
                {
                    NombreCampania = "Adopta con Responsabilidad",
                    TipoCampania = "Adopción",
                    DescripcionCampania = "Encuentra un nuevo integrante para tu familia y recibe asesoría para una adopción responsable.",
                    FechaInicio = today.AddDays(18),
                    FechaFin = today.AddDays(18),
                    HoraInicio = new TimeOnly(10, 0),
                    HoraFin = new TimeOnly(15, 0),
                    UbicacionCampania = "Parque Bicentenario",
                    ResponsableCampania = "Unidad de Bienestar Animal"
                });
        }

        if (!await context.Alertas.AnyAsync())
        {
            context.Alertas.AddRange(
                new Alerta
                {
                    TipoAlerta = "Cierre vial programado",
                    DescripcionAlerta = "Revisa rutas alternas por trabajos de mantenimiento vial.",
                    Fecha = DateTime.Now,
                    Sector = "Centro Histórico",
                    Latitud = -0.220164m,
                    Longitud = -78.512327m,
                    Direccion = "Centro Histórico de Quito",
                    EstadoAlerta = "Activa",
                    Cedula = "MUNICIPIO",
                    Correo = "alertas@quito.gob.ec",
                    Telefono = "1800-QUITO"
                },
                new Alerta
                {
                    TipoAlerta = "Jornada comunitaria",
                    DescripcionAlerta = "Limpieza y recuperación de espacios públicos este fin de semana.",
                    Fecha = DateTime.Now.AddHours(-3),
                    Sector = "Quitumbe",
                    Latitud = -0.291409m,
                    Longitud = -78.552928m,
                    Direccion = "Administración Zonal Quitumbe",
                    EstadoAlerta = "Activa",
                    Cedula = "MUNICIPIO",
                    Correo = "alertas@quito.gob.ec",
                    Telefono = "1800-QUITO"
                });
        }

        if (!await context.Reportes.AnyAsync())
        {
            context.Reportes.AddRange(
                new Reporte
                {
                    TipoReporte = "Alumbrado público",
                    DescripcionReporte = "Luminaria apagada desde hace varios días.",
                    Parroquia = "Chillogallo",
                    AdministracionZonal = "Quitumbe",
                    Latitud = -0.285m,
                    Longitud = -78.550m,
                    Direccion = "Av. Mariscal Sucre y calle secundaria",
                    EstadoReporte = "En revisión",
                    Cedula = CitizenCedula,
                    Correo = CitizenEmail,
                    Telefono = "0990000000"
                },
                new Reporte
                {
                    TipoReporte = "Bache o daño en vía",
                    DescripcionReporte = "Bache profundo que dificulta el tránsito vehicular.",
                    Parroquia = "Iñaquito",
                    AdministracionZonal = "Eugenio Espejo",
                    Latitud = -0.180m,
                    Longitud = -78.485m,
                    Direccion = "Av. 6 de Diciembre",
                    EstadoReporte = "Pendiente",
                    Cedula = CitizenCedula,
                    Correo = CitizenEmail,
                    Telefono = "0990000000"
                });
        }

        if (!await context.SituacionCalle.AnyAsync())
        {
            context.SituacionCalle.Add(new SituacionCalle
            {
                NombreReportante = "Ciudadano de prueba",
                Sector = "Centro",
                Latitud = -0.220m,
                Longitud = -78.512m,
                Direccion = "Plaza del Teatro",
                Condicion = "Persona adulta mayor",
                Descripcion = "Persona que requiere valoración social y atención prioritaria.",
                Prioridad = "Alta",
                RiesgoInmediato = true,
                Estado = "En seguimiento"
            });
        }

        if (!await context.ReportesViolencia.AnyAsync())
        {
            context.ReportesViolencia.Add(new ReporteViolencia
            {
                TipoViolencia = "Violencia intrafamiliar",
                Descripcion = "Alerta de demostración para seguimiento institucional.",
                Latitud = -0.205m,
                Longitud = -78.500m,
                Referencia = "Sector centro norte",
                Cedula = CitizenCedula,
                Correo = CitizenEmail,
                Telefono = "0990000000",
                Estado = "Recibido",
                FechaReporte = DateTime.Now.AddDays(-1)
            });
        }

        if (!await context.Obras.AnyAsync())
        {
            context.Obras.AddRange(
                new Obra
                {
                    NombreObra = "Mejoramiento del parque barrial",
                    DescripcionObra = "Rehabilitación de juegos, iluminación y áreas verdes.",
                    SectorObra = "Quitumbe",
                    Latitud = -0.291409m,
                    Longitud = -78.552928m,
                    EstadoObra = "En análisis"
                },
                new Obra
                {
                    NombreObra = "Cruce peatonal seguro",
                    DescripcionObra = "Señalización y semáforo peatonal junto a la unidad educativa.",
                    SectorObra = "La Magdalena",
                    Latitud = -0.240m,
                    Longitud = -78.530m,
                    EstadoObra = "Propuesta"
                });
        }

        if (!await context.Permisos.AnyAsync())
        {
            context.Permisos.Add(new Permiso
            {
                TipoPermiso = "Uso de espacio público",
                DescripcionPermiso = "Permiso de demostración para una actividad comunitaria.",
                EstadoPermiso = "Ingresado",
                DocumentoAdjunto = string.Empty,
                Observaciones = $"[CEDULA:{CitizenCedula}][CORREO:{CitizenEmail}]"
            });
        }

        if (!await context.Reservas.AnyAsync())
        {
            context.Reservas.Add(new Reserva
            {
                EspacioReserva = "Casa comunal",
                FechaReserva = today.AddDays(14),
                HoraInicio = new TimeOnly(15, 0),
                HoraFin = new TimeOnly(17, 0),
                MotivoReserva = "Reunión de planificación barrial.",
                EstadoReserva = "Solicitada",
                Observaciones = $"[CEDULA:{CitizenCedula}][CORREO:{CitizenEmail}]",
                DocumentoAdjunto = string.Empty
            });
        }

        if (!await context.Consultas.AnyAsync())
        {
            context.Consultas.AddRange(
                new Consulta
                {
                    Cedula = CitizenCedula,
                    ValorPendiente = 42.75m,
                    Pagado = false,
                    FechaConsulta = DateTime.Now
                },
                new Consulta
                {
                    Cedula = CitizenCedula,
                    ValorPendiente = 0m,
                    Pagado = true,
                    FechaConsulta = DateTime.Now.AddMonths(-1)
                });
        }

        if (!await context.Inscripciones.AnyAsync())
        {
            context.Inscripciones.Add(new Inscripcion
            {
                NombreActividad = "Voluntariado de recuperación de parques",
                DescripcionActividad = "Jornada comunitaria de limpieza y pintura.",
                NombreCompleto = "Ciudadano de prueba",
                Cedula = CitizenCedula,
                Correo = CitizenEmail,
                Telefono = "0990000000",
                Estado = "Activa"
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task EnsureUsersAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();

        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new Usuario
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                NombreCompleto = "Administrador Municipio",
                Cedula = "1700000001",
                Sector = "Administración Central",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, AdminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
        else if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var citizen = await userManager.FindByEmailAsync(CitizenEmail);
        if (citizen is null)
        {
            citizen = new Usuario
            {
                UserName = CitizenEmail,
                Email = CitizenEmail,
                NombreCompleto = "Ciudadano de Prueba",
                Cedula = CitizenCedula,
                Sector = "Quitumbe",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(citizen, CitizenPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(citizen, "Usuario");
        }
        else if (!await userManager.IsInRoleAsync(citizen, "Usuario"))
        {
            await userManager.AddToRoleAsync(citizen, "Usuario");
        }
    }
}
