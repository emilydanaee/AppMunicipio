using System.Collections.ObjectModel;
using System.Globalization;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class ManagementPage : ContentPage, IQueryAttributable
{
    private string _module = "taller";
    private bool _configured;
    private UserSession? _session;

    public ObservableCollection<ManagementCard> Items { get; } = new();

    public ManagementPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("module", out var value))
            _module = Uri.UnescapeDataString(value?.ToString() ?? "taller").ToLowerInvariant();

        ConfigureModule();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_configured)
            ConfigureModule();
        _session = await AppServices.Session.LoadAsync();
        UpdateMode();
        await LoadAsync();
    }

    private void ConfigureModule()
    {
        _configured = true;
        var (title, subtitle, color) = _module switch
        {
            "taller" => ("Casa Somos · Talleres", "Cronogramas, sectores, cupos e inscritos", "#9417F4"),
            "feria" => ("Ferias de Emprendimiento", "Ferias activas y administración de información", "#F46A0A"),
            "campania" => ("Bienestar Animal", "Campañas veterinarias y de adopción", "#20A464"),
            "inscripcion" => ("Inscripciones y Voluntariado", "Participación ciudadana en actividades", "#9417F4"),
            "reporte" => ("Reportes Ciudadanos", "Incidentes urbanos, estado y seguimiento", "#E42319"),
            "alerta" => ("Alertas de Seguridad", "Alertas comunitarias activas", "#E42319"),
            "calle" => ("Situación de Calle", "Registro, atención y cierre de casos", "#E42319"),
            "violencia" => ("Situaciones de Violencia", "Denuncias, alertas y seguimiento", "#E42319"),
            "obra" => ("Obras Prioritarias", "Presupuestos participativos para barrios", "#0878D1"),
            "reserva" => ("Reserva de Espacios", "Disponibilidad, modificación y cancelación", "#0878D1"),
            "permiso" => ("Permisos Municipales", "Requisitos, documentos y avance", "#0878D1"),
            "consulta" => ("Obligaciones e Impuestos", "Valores pendientes y consultas", "#0878D1"),
            _ => ("Gestión Municipal", "Crear, consultar, actualizar y eliminar", "#0878D1")
        };

        TitleLabel.Text = title;
        SubtitleLabel.Text = subtitle;
        HeroBorder.BackgroundColor = Color.FromArgb(color);
        NewButton.TextColor = Color.FromArgb(color);
    }

    private void UpdateMode()
    {
        var adminOnly = IsAdminOnlyModule();
        var isAdmin = _session?.IsAdmin == true;

        if (adminOnly && !isAdmin)
        {
            NewButton.IsEnabled = false;
            NewButton.Text = "🔒 Admin";
            ModeLabel.Text = "Puedes consultar la información. Inicia sesión como administrador para crear, editar o eliminar.";
        }
        else
        {
            NewButton.IsEnabled = true;
            NewButton.Text = "＋ Nuevo";
            ModeLabel.Text = isAdmin
                ? "Modo administrador: gestión completa habilitada."
                : "Modo ciudadano: puedes registrar y dar seguimiento a tus solicitudes.";
        }
    }

    private async Task LoadAsync()
    {
        SetLoading(true);
        Items.Clear();

        try
        {
            switch (_module)
            {
                case "taller": await LoadTalleresAsync(); break;
                case "feria": await LoadFeriasAsync(); break;
                case "campania": await LoadCampaniasAsync(); break;
                case "inscripcion": await LoadInscripcionesAsync(); break;
                case "reporte": await LoadReportesAsync(); break;
                case "alerta": await LoadAlertasAsync(); break;
                case "calle": await LoadCalleAsync(); break;
                case "violencia": await LoadViolenciaAsync(); break;
                case "obra": await LoadObrasAsync(); break;
                case "reserva": await LoadReservasAsync(); break;
                case "permiso": await LoadPermisosAsync(); break;
                case "consulta": await LoadConsultasAsync(); break;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Gestión", $"No se pudo cargar la información: {ex.Message}", "Aceptar");
        }
        finally
        {
            SetLoading(false);
            RefreshControl.IsRefreshing = false;
        }
    }

    private async Task LoadTalleresAsync()
    {
        var result = await AppServices.Api.GetListAsync<Taller>("/api/Taller");
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Taller>())
        {
            Items.Add(Card(item.IdTaller, "♙", item.NombreTaller, item.DescripcionTaller,
                $"{Date(item.FechaInicio)} · {Time(item.HoraInicio)}-{Time(item.HoraFin)} · {item.SectorTaller}",
                $"{Math.Max(0, item.CuposTaller - item.Inscritos)} cupos", "#9417F4", adminEdit: true,
                special: true, specialText: "Inscritos"));
        }
    }

    private async Task LoadFeriasAsync()
    {
        var result = await AppServices.Api.GetListAsync<Feria>("/api/Feria");
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Feria>())
            Items.Add(Card(item.IdFeria, "◆", item.NombreFeria, item.DescripcionFeria,
                $"{Date(item.FechaInicio)} · {item.SectorFeria} · {item.UbicacionFeria}",
                "Activa", "#F46A0A", adminEdit: true));
    }

    private async Task LoadCampaniasAsync()
    {
        var result = await AppServices.Api.GetListAsync<Campania>("/api/Campania");
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Campania>())
            Items.Add(Card(item.IdCampania, "♥", item.NombreCampania, item.DescripcionCampania,
                $"{Date(item.FechaInicio)} · {item.UbicacionCampania}",
                item.TipoCampania, "#20A464", adminEdit: true));
    }

    private async Task LoadInscripcionesAsync()
    {
        var endpoint = _session is not null && !_session.IsAdmin
            ? $"/api/Inscripcion/usuario/{Uri.EscapeDataString(_session.Cedula)}"
            : "/api/Inscripcion";
        var result = await AppServices.Api.GetListAsync<InscripcionGeneral>(endpoint);
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<InscripcionGeneral>())
            Items.Add(Card(item.IdInscripcion, "✓", item.NombreActividad, item.NombreCompleto,
                $"{item.Cedula} · {item.FechaInscripcion:dd MMM yyyy}",
                FirstNotEmpty(item.Estado, "Activa"), StatusColor(item.Estado)));
    }

    private async Task LoadReportesAsync()
    {
        var endpoint = _session is not null && !_session.IsAdmin
            ? $"/api/Reporte/usuario/{Uri.EscapeDataString(_session.Cedula)}"
            : "/api/Reporte";
        var result = await AppServices.Api.GetListAsync<Reporte>(endpoint);
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Reporte>())
            Items.Add(Card(item.IdReporte, "!", item.TipoReporte, item.DescripcionReporte,
                $"{item.FechaReporte:dd MMM yyyy} · {FirstNotEmpty(item.Direccion, item.Parroquia, "Sin dirección")}",
                FirstNotEmpty(item.EstadoReporte, "Pendiente"), StatusColor(item.EstadoReporte), adminEdit: true,
                special: HasLocation(item.Latitud, item.Longitud), specialText: "Mapa",
                latitude: item.Latitud, longitude: item.Longitud));
    }

    private async Task LoadAlertasAsync()
    {
        var result = await AppServices.Api.GetListAsync<Alerta>("/api/Alerta");
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Alerta>())
            Items.Add(Card(item.IdAlerta, "⚠", item.TipoAlerta, item.DescripcionAlerta,
                $"{item.Fecha:dd MMM yyyy HH:mm} · {item.Sector}",
                FirstNotEmpty(item.EstadoAlerta, "Activa"), StatusColor(item.EstadoAlerta),
                adminEdit: true, special: HasLocation(item.Latitud, item.Longitud), specialText: "Mapa",
                latitude: item.Latitud, longitude: item.Longitud));
    }

    private async Task LoadCalleAsync()
    {
        var result = await AppServices.Api.GetListAsync<SituacionCalle>("/api/SituacionCalle");
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<SituacionCalle>())
            Items.Add(Card(item.IdSituacionCalle, "♟", item.Condicion, item.Descripcion,
                $"{item.FechaReporte:dd MMM yyyy} · {item.Sector} · Prioridad {item.Prioridad}",
                FirstNotEmpty(item.Estado, "Pendiente"), StatusColor(item.Estado), adminEdit: true,
                special: HasLocation(item.Latitud, item.Longitud), specialText: "Mapa",
                latitude: item.Latitud, longitude: item.Longitud));
    }

    private async Task LoadViolenciaAsync()
    {
        var result = await AppServices.Api.GetListAsync<ReporteViolencia>("/api/ReporteViolencia");
        if (!await EnsureSuccess(result)) return;
        var values = result.Data ?? new List<ReporteViolencia>();
        if (_session is not null && !_session.IsAdmin)
            values = values.Where(v => v.Cedula == _session.Cedula).ToList();
        foreach (var item in values)
            Items.Add(Card(item.IdReporteViolencia, "◆", item.TipoViolencia, item.Descripcion,
                $"{item.FechaReporte:dd MMM yyyy} · {item.Referencia}",
                FirstNotEmpty(item.Estado, "Recibido"), StatusColor(item.Estado), adminEdit: true,
                special: HasLocation(item.Latitud, item.Longitud), specialText: "Mapa",
                latitude: item.Latitud, longitude: item.Longitud));
    }

    private async Task LoadObrasAsync()
    {
        var result = await AppServices.Api.GetListAsync<Obra>("/api/Obra");
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Obra>())
            Items.Add(Card(item.IdObra, "⌂", item.NombreObra, item.DescripcionObra,
                $"{item.FechaSolicitud:dd MMM yyyy} · {item.SectorObra}",
                FirstNotEmpty(item.EstadoObra, "Propuesta"), StatusColor(item.EstadoObra),
                special: HasLocation(item.Latitud, item.Longitud), specialText: "Mapa",
                latitude: item.Latitud, longitude: item.Longitud));
    }

    private async Task LoadReservasAsync()
    {
        var endpoint = _session is not null && !_session.IsAdmin
            ? $"/api/Reserva/usuario/{Uri.EscapeDataString(_session.Cedula)}"
            : "/api/Reserva";
        var result = await AppServices.Api.GetListAsync<Reserva>(endpoint);
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Reserva>())
            Items.Add(Card(item.IdReserva, "▣", item.EspacioReserva, item.MotivoReserva,
                $"{DateText(item.FechaReserva)} · {Time(item.HoraInicio)}-{Time(item.HoraFin)}",
                FirstNotEmpty(item.EstadoReserva, "Solicitada"), StatusColor(item.EstadoReserva),
                special: !string.IsNullOrWhiteSpace(item.DocumentoAdjunto), specialText: "Documento",
                resourceUrl: item.DocumentoAdjunto));
    }

    private async Task LoadPermisosAsync()
    {
        var endpoint = _session is not null && !_session.IsAdmin
            ? $"/api/Permiso/usuario/{Uri.EscapeDataString(_session.Cedula)}"
            : "/api/Permiso";
        var result = await AppServices.Api.GetListAsync<Permiso>(endpoint);
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Permiso>())
            Items.Add(Card(item.IdPermiso, "▤", item.TipoPermiso, item.DescripcionPermiso,
                item.FechaPermiso.ToString("dd MMM yyyy"),
                FirstNotEmpty(item.EstadoPermiso, "Ingresado"), StatusColor(item.EstadoPermiso),
                special: !string.IsNullOrWhiteSpace(item.DocumentoAdjunto), specialText: "Documento",
                resourceUrl: item.DocumentoAdjunto));
    }

    private async Task LoadConsultasAsync()
    {
        var endpoint = _session is not null && !_session.IsAdmin
            ? $"/api/Consulta/cedula/{Uri.EscapeDataString(_session.Cedula)}"
            : "/api/Consulta";
        var result = await AppServices.Api.GetListAsync<Consulta>(endpoint);
        if (!await EnsureSuccess(result)) return;
        foreach (var item in result.Data ?? new List<Consulta>())
            Items.Add(Card(item.IdConsulta, "$", $"Cédula {item.Cedula}",
                item.ValorPendiente.ToString("C2", CultureInfo.GetCultureInfo("es-EC")),
                item.FechaConsulta.ToString("dd MMM yyyy"),
                item.Pagado ? "Pagado" : "Pendiente", item.Pagado ? "#20A464" : "#F46A0A"));
    }

    private ManagementCard Card(
        int id, string icon, string title, string subtitle, string detail,
        string status, string statusColor, bool adminEdit = false,
        bool special = false, string specialText = "", double latitude = 0,
        double longitude = 0, string resourceUrl = "")
    {
        var canManage = !adminEdit || _session?.IsAdmin == true;
        return new ManagementCard
        {
            Id = id,
            Module = _module,
            Icon = icon,
            Title = title,
            Subtitle = subtitle,
            Detail = detail,
            Status = status,
            StatusColor = statusColor,
            CanEdit = canManage,
            CanDelete = canManage,
            ShowSpecialAction = special,
            SpecialActionText = specialText,
            Latitude = latitude,
            Longitude = longitude,
            ResourceUrl = resourceUrl
        };
    }

    private async Task<bool> EnsureSuccess<T>(ApiResult<List<T>> result)
    {
        if (result.IsSuccess) return true;
        await DisplayAlert("Conexión", result.Error.Trim('"'), "Aceptar");
        return false;
    }

    private async void OnNewClicked(object sender, EventArgs e)
    {
        _session = await AppServices.Session.LoadAsync();
        if (IsAdminOnlyModule() && _session?.IsAdmin != true)
        {
            await DisplayAlert("Acceso administrativo", "Inicia sesión con una cuenta administradora para gestionar este módulo.", "Aceptar");
            return;
        }
        if (_session is null && RequiresLogin())
        {
            var login = await DisplayAlert("Inicio de sesión", "Inicia sesión para registrar información.", "Iniciar sesión", "Cancelar");
            if (login) await Shell.Current.GoToAsync(nameof(LoginPage));
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(UniversalFormPage)}?module={Uri.EscapeDataString(_module)}&id=0");
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: ManagementCard item }) return;
        await Shell.Current.GoToAsync($"{nameof(UniversalFormPage)}?module={Uri.EscapeDataString(_module)}&id={item.Id}");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: ManagementCard item }) return;
        var confirm = await DisplayAlert("Confirmar", $"¿Deseas eliminar o cancelar “{item.Title}”?", "Sí", "No");
        if (!confirm) return;

        SetLoading(true);
        var result = await AppServices.Api.DeleteAsync(DeleteEndpoint(item.Id));
        SetLoading(false);
        if (!result.IsSuccess)
        {
            await DisplayAlert("No se pudo completar", result.Error.Trim('"'), "Aceptar");
            return;
        }

        Items.Remove(item);
        await DisplayAlert("Listo", "La operación se completó correctamente.", "Aceptar");
    }

    private async void OnSpecialClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: ManagementCard item }) return;

        if (_module == "taller")
        {
            await Shell.Current.GoToAsync($"{nameof(AttendeesPage)}?id={item.Id}&title={Uri.EscapeDataString(item.Title)}");
            return;
        }

        if (!string.IsNullOrWhiteSpace(item.ResourceUrl))
        {
            var url = AppServices.Api.BuildImageUrl(item.ResourceUrl);
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                await Launcher.Default.OpenAsync(uri);
            return;
        }

        if (HasLocation(item.Latitude, item.Longitude))
            await Map.Default.OpenAsync(item.Latitude, item.Longitude, new MapLaunchOptions { Name = item.Title });
    }

    private string DeleteEndpoint(int id) => _module switch
    {
        "taller" => $"/api/Taller/{id}",
        "feria" => $"/api/Feria/{id}",
        "campania" => $"/api/Campania/{id}",
        "inscripcion" => $"/api/Inscripcion/{id}",
        "reporte" => $"/api/Reporte/{id}",
        "alerta" => $"/api/Alerta/{id}",
        "calle" => $"/api/SituacionCalle/{id}",
        "violencia" => $"/api/ReporteViolencia/{id}",
        "obra" => $"/api/Obra/{id}",
        "reserva" => $"/api/Reserva/{id}",
        "permiso" => $"/api/Permiso/{id}",
        "consulta" => $"/api/Consulta/{id}",
        _ => $"/api/{_module}/{id}"
    };

    private bool IsAdminOnlyModule() => _module is "taller" or "feria" or "campania";
    private bool RequiresLogin() => _module is not "alerta";

    private async void OnRefreshing(object sender, EventArgs e) => await LoadAsync();

    private void SetLoading(bool value)
    {
        LoadingIndicator.IsVisible = value;
        LoadingIndicator.IsRunning = value;
        NewButton.IsEnabled = !value && (!IsAdminOnlyModule() || _session?.IsAdmin == true);
    }

    private static string Date(DateTime value) => value == default ? "Fecha pendiente" : value.ToString("dd MMM yyyy");
    private static string DateText(string value) => DateTime.TryParse(value, out var date) ? date.ToString("dd MMM yyyy") : value;
    private static string Time(string? value) => string.IsNullOrWhiteSpace(value) ? "--:--" : value.Length >= 5 ? value[..5] : value;
    private static bool HasLocation(double lat, double lng) => Math.Abs(lat) > 0.000001 || Math.Abs(lng) > 0.000001;
    private static string FirstNotEmpty(params string[] values) => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? string.Empty;

    private static string StatusColor(string? status)
    {
        var value = status?.ToLowerInvariant() ?? string.Empty;
        if (value.Contains("aprobad") || value.Contains("pagad") || value.Contains("cerrad") || value.Contains("complet") || value.Contains("activ")) return "#20A464";
        if (value.Contains("rechaz") || value.Contains("cancel")) return "#E42319";
        if (value.Contains("pend") || value.Contains("solicit") || value.Contains("ingres") || value.Contains("recibid")) return "#F46A0A";
        return "#0878D1";
    }
}
