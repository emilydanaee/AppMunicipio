using System.Globalization;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class UniversalFormPage : ContentPage, IQueryAttributable
{
    private string _module = "taller";
    private int _id;
    private bool _initialized;
    private Location? _location;
    private FileResult? _image;
    private FileResult? _document;
    private string _existingImage = string.Empty;
    private string _existingDocument = string.Empty;
    private UserSession? _session;

    public UniversalFormPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("module", out var moduleValue))
            _module = Uri.UnescapeDataString(moduleValue?.ToString() ?? "taller").ToLowerInvariant();
        if (query.TryGetValue("id", out var idValue))
            int.TryParse(idValue?.ToString(), out _id);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_initialized) return;
        _initialized = true;

        _session = await AppServices.Session.LoadAsync();
        ConfigureForm();
        FillSessionDefaults();
        if (_id > 0)
            await LoadExistingAsync();
    }

    private void ConfigureForm()
    {
        HideAllSections();
        StartDatePicker.Date = DateTime.Today.AddDays(7);
        EndDatePicker.Date = DateTime.Today.AddDays(7);
        StartTimePicker.Time = new TimeSpan(9, 0, 0);
        EndTimePicker.Time = new TimeSpan(11, 0, 0);
        ModalityPicker.ItemsSource = new List<string> { "Presencial", "Virtual", "Híbrida" };
        PriorityPicker.ItemsSource = new List<string> { "Baja", "Media", "Alta", "Crítica" };
        ConditionPicker.ItemsSource = new List<string> { "Niñez", "Persona adulta mayor", "Discapacidad", "Salud", "Consumo problemático", "Otro" };

        var editing = _id > 0;
        SubmitButton.Text = editing ? "Guardar cambios" : "Registrar";

        switch (_module)
        {
            case "taller":
                SetHeader(editing ? "Editar taller" : "Crear taller", "Casa Somos · cronograma, sector y cupos", "#9417F4");
                Show(NameSection, DescriptionSection, DatesSection, SectorSection, LocationSection, ActivityExtraSection, ImageSection);
                NameFieldLabel.Text = "Nombre del taller";
                InstructorField.IsVisible = DaysField.IsVisible = ModalityField.IsVisible = CapacityField.IsVisible = true;
                ResponsibleField.IsVisible = false;
                break;
            case "feria":
                SetHeader(editing ? "Editar feria" : "Registrar feria", "Emprendimientos y actividades vigentes", "#F46A0A");
                Show(NameSection, DescriptionSection, DatesSection, SectorSection, LocationSection, ImageSection);
                NameFieldLabel.Text = "Nombre de la feria";
                break;
            case "campania":
                SetHeader(editing ? "Editar campaña" : "Crear campaña", "Bienestar animal y adopción responsable", "#20A464");
                Show(NameSection, TypeSection, DescriptionSection, DatesSection, LocationSection, ActivityExtraSection, ImageSection);
                NameFieldLabel.Text = "Nombre de la campaña";
                TypeFieldLabel.Text = "Tipo de campaña";
                TypePicker.ItemsSource = new List<string> { "Atención veterinaria", "Vacunación", "Esterilización", "Adopción", "Educación", "Otro" };
                InstructorField.IsVisible = DaysField.IsVisible = ModalityField.IsVisible = CapacityField.IsVisible = false;
                ResponsibleField.IsVisible = true;
                break;
            case "inscripcion":
                SetHeader(editing ? "Actualizar inscripción" : "Registrar inscripción", "Actividades, voluntariado y participación", "#9417F4");
                Show(NameSection, DescriptionSection, PersonalSection, StatusSection);
                NameFieldLabel.Text = "Nombre de la actividad";
                StatusPicker.ItemsSource = new List<string> { "Activa", "Confirmada", "Pendiente", "Cancelada" };
                break;
            case "reporte":
                SetHeader(editing ? "Actualizar reporte" : "Crear reporte", "Incidentes urbanos con ubicación", "#E42319");
                Show(TypeSection, DescriptionSection, SectorSection, LocationSection, PersonalSection, StatusSection, ImageSection);
                TypeFieldLabel.Text = "Tipo de reporte";
                SectorFieldLabel.Text = "Administración zonal / parroquia";
                TypePicker.ItemsSource = new List<string> { "Bache o daño en vía", "Alumbrado público", "Basura o limpieza", "Ruido", "Seguridad ciudadana", "Otro" };
                StatusPicker.ItemsSource = StandardCaseStatuses();
                break;
            case "alerta":
                SetHeader(editing ? "Actualizar alerta" : "Crear alerta comunitaria", "Seguridad y prevención barrial", "#E42319");
                Show(TypeSection, DescriptionSection, SectorSection, LocationSection, PersonalSection, StatusSection, ImageSection);
                TypeFieldLabel.Text = "Tipo de alerta";
                TypePicker.ItemsSource = new List<string> { "Seguridad", "Cierre vial", "Riesgo natural", "Emergencia barrial", "Persona desaparecida", "Otro" };
                StatusPicker.ItemsSource = new List<string> { "Activa", "En atención", "Cerrada", "Cancelada" };
                break;
            case "calle":
                SetHeader(editing ? "Actualizar caso social" : "Reportar situación de calle", "Atención social y seguimiento", "#E42319");
                Show(NameSection, DescriptionSection, SectorSection, LocationSection, CaseSection, StatusSection);
                NameFieldLabel.Text = "Nombre del reportante";
                StatusPicker.ItemsSource = StandardCaseStatuses();
                break;
            case "violencia":
                SetHeader(editing ? "Actualizar seguimiento" : "Registrar alerta de violencia", "Registro reservado para atención institucional", "#E42319");
                Show(TypeSection, DescriptionSection, LocationSection, PersonalSection, StatusSection);
                TypeFieldLabel.Text = "Tipo de violencia";
                LocationFieldLabel.Text = "Referencia y ubicación";
                TypePicker.ItemsSource = new List<string> { "Violencia intrafamiliar", "Violencia de género", "Acoso", "Violencia sexual", "Violencia psicológica", "Otro" };
                StatusPicker.ItemsSource = StandardCaseStatuses();
                break;
            case "obra":
                SetHeader(editing ? "Editar propuesta" : "Solicitar obra prioritaria", "Presupuestos participativos para tu barrio", "#0878D1");
                Show(NameSection, DescriptionSection, SectorSection, LocationSection, StatusSection);
                NameFieldLabel.Text = "Nombre de la obra";
                StatusPicker.ItemsSource = new List<string> { "Propuesta", "En análisis", "Priorizada", "En ejecución", "Completada", "Rechazada" };
                break;
            case "reserva":
                SetHeader(editing ? "Modificar reserva" : "Crear reserva", "Espacios públicos y disponibilidad", "#0878D1");
                Show(NameSection, DescriptionSection, DatesSection, StatusSection, ObservationsSection, DocumentSection);
                NameFieldLabel.Text = "Espacio público";
                DescriptionFieldLabel.Text = "Motivo de la reserva";
                StatusPicker.ItemsSource = new List<string> { "Solicitada", "En revisión", "Aprobada", "Rechazada", "Cancelada" };
                break;
            case "permiso":
                SetHeader(editing ? "Actualizar permiso" : "Solicitar permiso", "Trámite, requisitos y documentos", "#0878D1");
                Show(TypeSection, DescriptionSection, StatusSection, ObservationsSection, DocumentSection);
                TypeFieldLabel.Text = "Tipo de permiso";
                TypePicker.ItemsSource = new List<string> { "Permiso de funcionamiento", "Uso de espacio público", "Construcción", "Publicidad", "Actividad económica", "Otro" };
                StatusPicker.ItemsSource = new List<string> { "Ingresado", "En revisión", "Observado", "Aprobado", "Rechazado", "Cancelado" };
                break;
            case "consulta":
                SetHeader(editing ? "Actualizar obligación" : "Registrar consulta", "Valores pendientes e información tributaria", "#0878D1");
                Show(PersonalSection, TaxSection);
                FullNameEntry.IsVisible = EmailEntry.IsVisible = PhoneEntry.IsVisible = false;
                break;
        }
    }

    private void HideAllSections()
    {
        foreach (var view in new VisualElement[]
        {
            NameSection, TypeSection, DescriptionSection, DatesSection, SectorSection,
            LocationSection, ActivityExtraSection, PersonalSection, CaseSection,
            StatusSection, TaxSection, ObservationsSection, DocumentSection, ImageSection
        })
            view.IsVisible = false;

        FullNameEntry.IsVisible = CedulaEntry.IsVisible = EmailEntry.IsVisible = PhoneEntry.IsVisible = true;
        InstructorField.IsVisible = DaysField.IsVisible = ModalityField.IsVisible = ResponsibleField.IsVisible = CapacityField.IsVisible = true;
    }

    private static void Show(params VisualElement[] views)
    {
        foreach (var view in views) view.IsVisible = true;
    }

    private void SetHeader(string title, string subtitle, string color)
    {
        TitleLabel.Text = title;
        SubtitleLabel.Text = subtitle;
        HeroBorder.BackgroundColor = Color.FromArgb(color);
        SubmitButton.BackgroundColor = Color.FromArgb(color);
        LoadingIndicator.Color = Color.FromArgb(color);
    }

    private void FillSessionDefaults()
    {
        if (_session is null) return;
        FullNameEntry.Text = _session.Nombre;
        CedulaEntry.Text = _session.Cedula;
        EmailEntry.Text = _session.Email;
        if (string.IsNullOrWhiteSpace(SectorEntry.Text)) SectorEntry.Text = _session.Sector;
        SelectPicker(StatusPicker, DefaultStatus());
    }

    private async Task LoadExistingAsync()
    {
        SetLoading(true);
        try
        {
            switch (_module)
            {
                case "taller":
                    var taller = await AppServices.Api.GetAsync<Taller>($"/api/Taller/{_id}");
                    if (await LoadOk(taller)) Fill(taller.Data!);
                    break;
                case "feria":
                    var feria = await AppServices.Api.GetAsync<Feria>($"/api/Feria/{_id}");
                    if (await LoadOk(feria)) Fill(feria.Data!);
                    break;
                case "campania":
                    var campaign = await AppServices.Api.GetAsync<Campania>($"/api/Campania/{_id}");
                    if (await LoadOk(campaign)) Fill(campaign.Data!);
                    break;
                case "inscripcion":
                    var registration = await AppServices.Api.GetAsync<InscripcionGeneral>($"/api/Inscripcion/{_id}");
                    if (await LoadOk(registration)) Fill(registration.Data!);
                    break;
                case "reporte":
                    var report = await AppServices.Api.GetAsync<Reporte>($"/api/Reporte/{_id}");
                    if (await LoadOk(report)) Fill(report.Data!);
                    break;
                case "alerta":
                    var alert = await AppServices.Api.GetAsync<Alerta>($"/api/Alerta/{_id}");
                    if (await LoadOk(alert)) Fill(alert.Data!);
                    break;
                case "calle":
                    var street = await AppServices.Api.GetAsync<SituacionCalle>($"/api/SituacionCalle/{_id}");
                    if (await LoadOk(street)) Fill(street.Data!);
                    break;
                case "violencia":
                    var violence = await AppServices.Api.GetAsync<ReporteViolencia>($"/api/ReporteViolencia/{_id}");
                    if (await LoadOk(violence)) Fill(violence.Data!);
                    break;
                case "obra":
                    var work = await AppServices.Api.GetAsync<Obra>($"/api/Obra/{_id}");
                    if (await LoadOk(work)) Fill(work.Data!);
                    break;
                case "reserva":
                    var reservation = await AppServices.Api.GetAsync<Reserva>($"/api/Reserva/{_id}");
                    if (await LoadOk(reservation)) Fill(reservation.Data!);
                    break;
                case "permiso":
                    var permit = await AppServices.Api.GetAsync<Permiso>($"/api/Permiso/{_id}");
                    if (await LoadOk(permit)) Fill(permit.Data!);
                    break;
                case "consulta":
                    var tax = await AppServices.Api.GetAsync<Consulta>($"/api/Consulta/{_id}");
                    if (await LoadOk(tax)) Fill(tax.Data!);
                    break;
            }
        }
        finally
        {
            SetLoading(false);
        }
    }

    private async Task<bool> LoadOk<T>(ApiResult<T> result)
    {
        if (result.IsSuccess && result.Data is not null) return true;
        ShowError(result.Error);
        await Task.CompletedTask;
        return false;
    }

    private void Fill(Taller x)
    {
        NameEntry.Text = x.NombreTaller; DescriptionEditor.Text = x.DescripcionTaller;
        SetDate(StartDatePicker, x.FechaInicio); SetDate(EndDatePicker, x.FechaFin);
        SetTime(StartTimePicker, x.HoraInicio); SetTime(EndTimePicker, x.HoraFin);
        SectorEntry.Text = x.SectorTaller; LocationEntry.Text = x.UbicacionTaller;
        InstructorEntry.Text = x.Instructor; DaysEntry.Text = x.Dias; SelectPicker(ModalityPicker, x.Modalidad);
        CapacityEntry.Text = x.CuposTaller.ToString(CultureInfo.InvariantCulture); _existingImage = x.ImagenTaller ?? string.Empty;
    }

    private void Fill(Feria x)
    {
        NameEntry.Text = x.NombreFeria; DescriptionEditor.Text = x.DescripcionFeria;
        SetDate(StartDatePicker, x.FechaInicio); SetDate(EndDatePicker, x.FechaFin);
        SetTime(StartTimePicker, x.HoraInicio); SetTime(EndTimePicker, x.HoraFin);
        SectorEntry.Text = x.SectorFeria; LocationEntry.Text = x.UbicacionFeria; _existingImage = x.ImagenFeria ?? string.Empty;
    }

    private void Fill(Campania x)
    {
        NameEntry.Text = x.NombreCampania; SelectPicker(TypePicker, x.TipoCampania); DescriptionEditor.Text = x.DescripcionCampania;
        SetDate(StartDatePicker, x.FechaInicio); SetDate(EndDatePicker, x.FechaFin);
        SetTime(StartTimePicker, x.HoraInicio); SetTime(EndTimePicker, x.HoraFin);
        LocationEntry.Text = x.UbicacionCampania; ResponsibleEntry.Text = x.ResponsableCampania; _existingImage = x.ImagenCampania ?? string.Empty;
    }

    private void Fill(InscripcionGeneral x)
    {
        NameEntry.Text = x.NombreActividad; DescriptionEditor.Text = x.DescripcionActividad; FullNameEntry.Text = x.NombreCompleto;
        CedulaEntry.Text = x.Cedula; EmailEntry.Text = x.Correo; PhoneEntry.Text = x.Telefono; SelectPicker(StatusPicker, x.Estado);
    }

    private void Fill(Reporte x)
    {
        SelectPicker(TypePicker, x.TipoReporte); DescriptionEditor.Text = x.DescripcionReporte;
        SectorEntry.Text = FirstNotEmpty(x.AdministracionZonal, x.Parroquia); LocationEntry.Text = x.Direccion;
        CedulaEntry.Text = x.Cedula; EmailEntry.Text = x.Correo; PhoneEntry.Text = x.Telefono;
        _location = new Location(x.Latitud, x.Longitud); UpdateGpsLabel(); SelectPicker(StatusPicker, x.EstadoReporte); _existingImage = x.ImagenReporte ?? string.Empty;
    }

    private void Fill(Alerta x)
    {
        SelectPicker(TypePicker, x.TipoAlerta); DescriptionEditor.Text = x.DescripcionAlerta; SectorEntry.Text = x.Sector;
        LocationEntry.Text = x.Direccion; CedulaEntry.Text = x.Cedula; EmailEntry.Text = x.Correo; PhoneEntry.Text = x.Telefono;
        _location = new Location(x.Latitud, x.Longitud); UpdateGpsLabel(); SelectPicker(StatusPicker, x.EstadoAlerta); _existingImage = x.ImagenAlerta ?? string.Empty;
    }

    private void Fill(SituacionCalle x)
    {
        NameEntry.Text = x.NombreReportante; DescriptionEditor.Text = x.Descripcion; SectorEntry.Text = x.Sector; LocationEntry.Text = x.Direccion;
        SelectPicker(ConditionPicker, x.Condicion); SelectPicker(PriorityPicker, x.Prioridad); RiskSwitch.IsToggled = x.RiesgoInmediato;
        _location = new Location(x.Latitud, x.Longitud); UpdateGpsLabel(); SelectPicker(StatusPicker, x.Estado);
    }

    private void Fill(ReporteViolencia x)
    {
        SelectPicker(TypePicker, x.TipoViolencia); DescriptionEditor.Text = x.Descripcion; LocationEntry.Text = x.Referencia;
        CedulaEntry.Text = x.Cedula; EmailEntry.Text = x.Correo; PhoneEntry.Text = x.Telefono;
        _location = new Location(x.Latitud, x.Longitud); UpdateGpsLabel(); SelectPicker(StatusPicker, x.Estado);
    }

    private void Fill(Obra x)
    {
        NameEntry.Text = x.NombreObra; DescriptionEditor.Text = x.DescripcionObra; SectorEntry.Text = x.SectorObra;
        _location = new Location(x.Latitud, x.Longitud); UpdateGpsLabel(); SelectPicker(StatusPicker, x.EstadoObra);
    }

    private void Fill(Reserva x)
    {
        NameEntry.Text = x.EspacioReserva; DescriptionEditor.Text = x.MotivoReserva;
        if (DateTime.TryParse(x.FechaReserva, out var date)) StartDatePicker.Date = EndDatePicker.Date = date;
        SetTime(StartTimePicker, x.HoraInicio); SetTime(EndTimePicker, x.HoraFin);
        SelectPicker(StatusPicker, x.EstadoReserva); ObservationsEditor.Text = CleanMetadata(x.Observaciones);
        _existingDocument = x.DocumentoAdjunto; DocumentLabel.Text = FileDisplay(_existingDocument);
    }

    private void Fill(Permiso x)
    {
        SelectPicker(TypePicker, x.TipoPermiso); DescriptionEditor.Text = x.DescripcionPermiso; SelectPicker(StatusPicker, x.EstadoPermiso);
        ObservationsEditor.Text = CleanMetadata(x.Observaciones); _existingDocument = x.DocumentoAdjunto; DocumentLabel.Text = FileDisplay(_existingDocument);
    }

    private void Fill(Consulta x)
    {
        CedulaEntry.Text = x.Cedula; AmountEntry.Text = x.ValorPendiente.ToString(CultureInfo.InvariantCulture); PaidSwitch.IsToggled = x.Pagado;
    }

    private async void OnGpsClicked(object sender, EventArgs e)
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("GPS", "Activa el permiso de ubicación para continuar.", "Aceptar");
                return;
            }

            GpsButton.IsEnabled = false;
            GpsLabel.Text = "Obteniendo ubicación...";
            _location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(18)))
                        ?? await Geolocation.Default.GetLastKnownLocationAsync();
            if (_location is null)
            {
                GpsLabel.Text = "No fue posible obtener la ubicación.";
                return;
            }
            UpdateGpsLabel();
            await FillAddressAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("GPS", $"No se pudo obtener la ubicación: {ex.Message}", "Aceptar");
        }
        finally
        {
            GpsButton.IsEnabled = true;
        }
    }

    private void UpdateGpsLabel()
    {
        if (_location is null) return;
        GpsLabel.Text = $"✓ {_location.Latitude:F6}, {_location.Longitude:F6}";
        GpsLabel.TextColor = Color.FromArgb("#168A52");
    }

    private async Task FillAddressAsync()
    {
        if (_location is null || !string.IsNullOrWhiteSpace(LocationEntry.Text)) return;
        try
        {
            var place = (await Geocoding.Default.GetPlacemarksAsync(_location.Latitude, _location.Longitude)).FirstOrDefault();
            if (place is not null)
                LocationEntry.Text = FirstNotEmpty(place.FeatureName, place.Thoroughfare, place.Locality, "Ubicación GPS");
        }
        catch { }
    }

    private async void OnImageClicked(object sender, EventArgs e)
    {
        try
        {
            var option = await DisplayActionSheet("Imagen", "Cancelar", null, "Tomar fotografía", "Elegir de galería");
            if (option == "Tomar fotografía")
            {
                var permission = await Permissions.RequestAsync<Permissions.Camera>();
                if (permission != PermissionStatus.Granted) return;
                _image = await MediaPicker.Default.CapturePhotoAsync();
            }
            else if (option == "Elegir de galería")
            {
                _image = await MediaPicker.Default.PickPhotoAsync();
            }
            ImageLabel.Text = _image?.FileName ?? FileDisplay(_existingImage);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Imagen", $"No se pudo seleccionar la imagen: {ex.Message}", "Aceptar");
        }
    }

    private async void OnDocumentClicked(object sender, EventArgs e)
    {
        try
        {
            _document = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Selecciona PDF, Word o imagen" });
            DocumentLabel.Text = _document?.FileName ?? FileDisplay(_existingDocument);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Documento", $"No se pudo seleccionar el archivo: {ex.Message}", "Aceptar");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        _session = await AppServices.Session.LoadAsync();

        if (IsAdminOnly() && _session?.IsAdmin != true)
        {
            ShowError("Esta operación requiere una cuenta administradora.");
            return;
        }

        var validation = ValidateForm();
        if (!string.IsNullOrWhiteSpace(validation))
        {
            ShowError(validation);
            return;
        }

        SetLoading(true);
        try
        {
            if (_document is not null)
            {
                var upload = await AppServices.Api.PostMultipartAsync<DocumentUploadResponse>(
                    "/api/Documento", new Dictionary<string, string?>(), _document, "archivo");
                if (!upload.IsSuccess)
                {
                    ShowError(upload.Error);
                    return;
                }
                _existingDocument = upload.Data?.Path ?? string.Empty;
            }

            var result = await SaveAsync();
            if (!result.IsSuccess)
            {
                ShowError(result.Error);
                return;
            }

            await DisplayAlert("Guardado", "La información se registró correctamente.", "Aceptar");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ShowError($"No se pudo guardar: {ex.Message}");
        }
        finally
        {
            SetLoading(false);
        }
    }

    private Task<ApiResult<object>> SaveAsync() => _module switch
    {
        "taller" => SaveTallerAsync(),
        "feria" => SaveFeriaAsync(),
        "campania" => SaveCampaniaAsync(),
        "inscripcion" => SaveInscripcionAsync(),
        "reporte" => SaveReporteAsync(),
        "alerta" => SaveAlertaAsync(),
        "calle" => SaveCalleAsync(),
        "violencia" => SaveViolenciaAsync(),
        "obra" => SaveObraAsync(),
        "reserva" => SaveReservaAsync(),
        "permiso" => SavePermisoAsync(),
        "consulta" => SaveConsultaAsync(),
        _ => Task.FromResult(ApiResult<object>.Failure("Módulo no reconocido."))
    };

    private Task<ApiResult<object>> SaveTallerAsync()
    {
        var fields = ActivityFields();
        fields["NombreTaller"] = Text(NameEntry); fields["DescripcionTaller"] = Text(DescriptionEditor);
        fields["SectorTaller"] = Text(SectorEntry); fields["UbicacionTaller"] = Text(LocationEntry);
        fields["Instructor"] = Text(InstructorEntry); fields["Dias"] = Text(DaysEntry);
        fields["Modalidad"] = Selected(ModalityPicker); fields["CuposTaller"] = Text(CapacityEntry);
        if (_id == 0) return AppServices.Api.PostMultipartAsync<object>("/api/Taller", fields, _image, "Imagen");

        var json = ToObjectDictionary(fields);
        json["IdTaller"] = _id; json["ImagenTaller"] = _existingImage;
        return AppServices.Api.PutJsonAsync<Dictionary<string, object?>, object>($"/api/Taller/{_id}", json);
    }

    private Task<ApiResult<object>> SaveFeriaAsync()
    {
        var fields = ActivityFields();
        fields["NombreFeria"] = Text(NameEntry); fields["DescripcionFeria"] = Text(DescriptionEditor);
        fields["SectorFeria"] = Text(SectorEntry); fields["UbicacionFeria"] = Text(LocationEntry);
        if (_id == 0) return AppServices.Api.PostMultipartAsync<object>("/api/Feria", fields, _image, "Imagen");

        var json = ToObjectDictionary(fields);
        json["IdFeria"] = _id; json["ImagenFeria"] = _existingImage;
        return AppServices.Api.PutJsonAsync<Dictionary<string, object?>, object>($"/api/Feria/{_id}", json);
    }

    private Task<ApiResult<object>> SaveCampaniaAsync()
    {
        var fields = ActivityFields();
        fields["NombreCampania"] = Text(NameEntry); fields["TipoCampania"] = Selected(TypePicker);
        fields["DescripcionCampania"] = Text(DescriptionEditor); fields["UbicacionCampania"] = Text(LocationEntry);
        fields["ResponsableCampania"] = Text(ResponsibleEntry);
        if (_id == 0) return AppServices.Api.PostMultipartAsync<object>("/api/Campania", fields, _image, "Imagen");

        var json = ToObjectDictionary(fields);
        json["IdCampania"] = _id; json["ImagenCampania"] = _existingImage;
        return AppServices.Api.PutJsonAsync<Dictionary<string, object?>, object>($"/api/Campania/{_id}", json);
    }

    private Task<ApiResult<object>> SaveInscripcionAsync()
    {
        var value = new InscripcionGeneral
        {
            IdInscripcion = _id,
            NombreActividad = Text(NameEntry),
            DescripcionActividad = Text(DescriptionEditor),
            NombreCompleto = Text(FullNameEntry),
            Cedula = Text(CedulaEntry),
            Correo = Text(EmailEntry),
            Telefono = Text(PhoneEntry),
            Estado = Selected(StatusPicker, "Activa"),
            FechaInscripcion = DateTime.Now
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<InscripcionGeneral, object>("/api/Inscripcion", value)
            : AppServices.Api.PutJsonAsync<InscripcionGeneral, object>($"/api/Inscripcion/{_id}", value);
    }

    private async Task<ApiResult<object>> SaveReporteAsync()
    {
        var fields = LocationFields();
        fields["TipoReporte"] = Selected(TypePicker); fields["DescripcionReporte"] = Text(DescriptionEditor);
        fields["AdministracionZonal"] = Text(SectorEntry); fields["Parroquia"] = Text(SectorEntry);
        AddPersonFields(fields);
        if (_id == 0) return await AppServices.Api.PostMultipartAsync<object>("/api/Reporte", fields, _image, "Imagen");

        var update = ToObjectDictionary(fields);
        var result = await AppServices.Api.PutJsonAsync<Dictionary<string, object?>, object>($"/api/Reporte/{_id}", update);
        if (result.IsSuccess && !string.IsNullOrWhiteSpace(Selected(StatusPicker)))
            result = await AppServices.Api.PatchJsonAsync<Dictionary<string, string>, object>($"/api/Reporte/{_id}/estado", new Dictionary<string, string> { ["EstadoReporte"] = Selected(StatusPicker) });
        return result;
    }

    private Task<ApiResult<object>> SaveAlertaAsync()
    {
        var fields = LocationFields();
        fields["TipoAlerta"] = Selected(TypePicker); fields["DescripcionAlerta"] = Text(DescriptionEditor);
        fields["Sector"] = Text(SectorEntry); fields["EstadoAlerta"] = Selected(StatusPicker, "Activa");
        AddPersonFields(fields);
        if (_id == 0) return AppServices.Api.PostMultipartAsync<object>("/api/Alerta", fields, _image, "Imagen");

        var json = ToObjectDictionary(fields);
        json["IdAlerta"] = _id; json["Fecha"] = DateTime.Now; json["ImagenAlerta"] = _existingImage;
        return AppServices.Api.PutJsonAsync<Dictionary<string, object?>, object>($"/api/Alerta/{_id}", json);
    }

    private Task<ApiResult<object>> SaveCalleAsync()
    {
        var value = new SituacionCalle
        {
            IdSituacionCalle = _id,
            NombreReportante = Text(NameEntry), Sector = Text(SectorEntry),
            Latitud = _location?.Latitude ?? 0, Longitud = _location?.Longitude ?? 0,
            Direccion = Text(LocationEntry), Condicion = Selected(ConditionPicker),
            Descripcion = Text(DescriptionEditor), Prioridad = Selected(PriorityPicker),
            RiesgoInmediato = RiskSwitch.IsToggled, Estado = Selected(StatusPicker, "Pendiente"),
            FechaReporte = DateTime.Now
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<SituacionCalle, object>("/api/SituacionCalle", value)
            : AppServices.Api.PutJsonAsync<SituacionCalle, object>($"/api/SituacionCalle/{_id}", value);
    }

    private Task<ApiResult<object>> SaveViolenciaAsync()
    {
        var value = new ReporteViolencia
        {
            IdReporteViolencia = _id, TipoViolencia = Selected(TypePicker), Descripcion = Text(DescriptionEditor),
            Latitud = _location?.Latitude ?? 0, Longitud = _location?.Longitude ?? 0, Referencia = Text(LocationEntry),
            Cedula = Text(CedulaEntry), Correo = Text(EmailEntry), Telefono = Text(PhoneEntry),
            Estado = Selected(StatusPicker, "Recibido"), FechaReporte = DateTime.Now
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<ReporteViolencia, object>("/api/ReporteViolencia", value)
            : AppServices.Api.PutJsonAsync<ReporteViolencia, object>($"/api/ReporteViolencia/{_id}", value);
    }

    private Task<ApiResult<object>> SaveObraAsync()
    {
        var value = new Obra
        {
            IdObra = _id, NombreObra = Text(NameEntry), DescripcionObra = Text(DescriptionEditor), SectorObra = Text(SectorEntry),
            Latitud = _location?.Latitude ?? 0, Longitud = _location?.Longitude ?? 0,
            EstadoObra = Selected(StatusPicker, "Propuesta"), FechaSolicitud = DateTime.Now
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<Obra, object>("/api/Obra", value)
            : AppServices.Api.PutJsonAsync<Obra, object>($"/api/Obra/{_id}", value);
    }

    private Task<ApiResult<object>> SaveReservaAsync()
    {
        var metadata = UserMetadata();
        var notes = Text(ObservationsEditor);
        var value = new Reserva
        {
            IdReserva = _id, EspacioReserva = Text(NameEntry), FechaReserva = StartDatePicker.Date.ToString("yyyy-MM-dd"),
            HoraInicio = TimeOnly.FromTimeSpan(StartTimePicker.Time).ToString("HH:mm:ss"),
            HoraFin = TimeOnly.FromTimeSpan(EndTimePicker.Time).ToString("HH:mm:ss"),
            MotivoReserva = Text(DescriptionEditor), EstadoReserva = Selected(StatusPicker, "Solicitada"),
            Observaciones = string.IsNullOrWhiteSpace(notes) ? metadata : $"{metadata} {notes}", DocumentoAdjunto = _existingDocument
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<Reserva, object>("/api/Reserva", value)
            : AppServices.Api.PutJsonAsync<Reserva, object>($"/api/Reserva/{_id}", value);
    }

    private Task<ApiResult<object>> SavePermisoAsync()
    {
        var metadata = UserMetadata();
        var notes = Text(ObservationsEditor);
        var value = new Permiso
        {
            IdPermiso = _id, TipoPermiso = Selected(TypePicker), DescripcionPermiso = Text(DescriptionEditor),
            FechaPermiso = DateTime.Now, EstadoPermiso = Selected(StatusPicker, "Ingresado"),
            DocumentoAdjunto = _existingDocument, Observaciones = string.IsNullOrWhiteSpace(notes) ? metadata : $"{metadata} {notes}"
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<Permiso, object>("/api/Permiso", value)
            : AppServices.Api.PutJsonAsync<Permiso, object>($"/api/Permiso/{_id}", value);
    }

    private Task<ApiResult<object>> SaveConsultaAsync()
    {
        decimal.TryParse(Text(AmountEntry).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount);
        var value = new Consulta
        {
            IdConsulta = _id, Cedula = Text(CedulaEntry), ValorPendiente = amount,
            Pagado = PaidSwitch.IsToggled, FechaConsulta = DateTime.Now
        };
        return _id == 0
            ? AppServices.Api.PostJsonAsync<Consulta, object>("/api/Consulta", value)
            : AppServices.Api.PutJsonAsync<Consulta, object>($"/api/Consulta/{_id}", value);
    }

    private Dictionary<string, string?> ActivityFields() => new()
    {
        ["FechaInicio"] = StartDatePicker.Date.ToString("yyyy-MM-dd"),
        ["FechaFin"] = EndDatePicker.Date.ToString("yyyy-MM-dd"),
        ["HoraInicio"] = TimeOnly.FromTimeSpan(StartTimePicker.Time).ToString("HH:mm:ss"),
        ["HoraFin"] = TimeOnly.FromTimeSpan(EndTimePicker.Time).ToString("HH:mm:ss")
    };

    private Dictionary<string, string?> LocationFields() => new()
    {
        ["Latitud"] = (_location?.Latitude ?? 0).ToString(CultureInfo.InvariantCulture),
        ["Longitud"] = (_location?.Longitude ?? 0).ToString(CultureInfo.InvariantCulture),
        ["Direccion"] = Text(LocationEntry)
    };

    private void AddPersonFields(Dictionary<string, string?> fields)
    {
        fields["Cedula"] = Text(CedulaEntry); fields["Correo"] = Text(EmailEntry); fields["Telefono"] = Text(PhoneEntry);
    }

    private string ValidateForm()
    {
        if (NameSection.IsVisible && string.IsNullOrWhiteSpace(Text(NameEntry))) return $"Completa: {NameFieldLabel.Text}.";
        if (TypeSection.IsVisible && string.IsNullOrWhiteSpace(Selected(TypePicker))) return $"Selecciona: {TypeFieldLabel.Text}.";
        if (DescriptionSection.IsVisible && Text(DescriptionEditor).Length < 5) return "Escribe una descripción de al menos 5 caracteres.";
        if (DatesSection.IsVisible && EndTimePicker.Time <= StartTimePicker.Time) return "La hora final debe ser posterior a la inicial.";
        if (DatesSection.IsVisible && _module is not "reserva" && EndDatePicker.Date < StartDatePicker.Date) return "La fecha final no puede ser anterior a la inicial.";
        if (SectorSection.IsVisible && string.IsNullOrWhiteSpace(Text(SectorEntry))) return "Completa el sector.";
        if (LocationSection.IsVisible && string.IsNullOrWhiteSpace(Text(LocationEntry)) && _location is null) return "Ingresa una referencia o captura la ubicación GPS.";
        if (PersonalSection.IsVisible && CedulaEntry.IsVisible && (Text(CedulaEntry).Length != 10 || !Text(CedulaEntry).All(char.IsDigit))) return "La cédula debe tener 10 dígitos.";
        if (PersonalSection.IsVisible && EmailEntry.IsVisible && !Text(EmailEntry).Contains('@')) return "Ingresa un correo válido.";
        if (_module == "taller" && (!int.TryParse(Text(CapacityEntry), out var capacity) || capacity <= 0)) return "Ingresa una cantidad de cupos válida.";
        if (_module == "consulta" && !decimal.TryParse(Text(AmountEntry).Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out _)) return "Ingresa un valor válido.";
        return string.Empty;
    }

    private bool IsAdminOnly() => _module is "taller" or "feria" or "campania";
    private string DefaultStatus() => _module switch
    {
        "inscripcion" => "Activa", "reporte" => "Pendiente", "alerta" => "Activa", "calle" => "Pendiente",
        "violencia" => "Recibido", "obra" => "Propuesta", "reserva" => "Solicitada", "permiso" => "Ingresado", _ => string.Empty
    };

    private string UserMetadata() => _session is null ? string.Empty : $"[CEDULA:{_session.Cedula}][CORREO:{_session.Email}]";

    private void ShowError(string message)
    {
        ErrorLabel.Text = string.IsNullOrWhiteSpace(message) ? "No se pudo completar la operación." : message.Trim('"');
        ErrorLabel.IsVisible = true;
    }

    private void SetLoading(bool value)
    {
        LoadingIndicator.IsVisible = LoadingIndicator.IsRunning = value;
        SubmitButton.IsEnabled = !value;
        GpsButton.IsEnabled = !value;
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");

    private static List<string> StandardCaseStatuses() => new() { "Pendiente", "Recibido", "En revisión", "En atención", "En seguimiento", "Cerrado", "Cancelado" };
    private static string Text(InputView view) => view.Text?.Trim() ?? string.Empty;
    private static string Selected(Picker picker, string fallback = "") => picker.SelectedItem?.ToString()?.Trim() ?? fallback;
    private static Dictionary<string, object?> ToObjectDictionary(Dictionary<string, string?> source) => source.ToDictionary(item => item.Key, item => (object?)item.Value);
    private static string FirstNotEmpty(params string?[] values) => values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;
    private static string FileDisplay(string path) => string.IsNullOrWhiteSpace(path) ? "No se seleccionó un archivo" : Path.GetFileName(path);
    private static string CleanMetadata(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var result = System.Text.RegularExpressions.Regex.Replace(value, @"\[(CEDULA|CORREO):[^\]]*\]", string.Empty);
        return result.Trim();
    }

    private static void SetDate(DatePicker picker, DateTime value)
    {
        if (value != default) picker.Date = value;
    }

    private static void SetTime(TimePicker picker, string? value)
    {
        if (TimeSpan.TryParse(value, out var time)) picker.Time = time;
    }

    private static void SelectPicker(Picker picker, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        var values = (picker.ItemsSource as IEnumerable<string>)?.ToList() ?? new List<string>();
        var match = values.FirstOrDefault(item => string.Equals(item, value, StringComparison.OrdinalIgnoreCase));
        if (match is null)
        {
            values.Add(value);
            picker.ItemsSource = values;
            match = value;
        }
        picker.SelectedItem = match;
    }
}
