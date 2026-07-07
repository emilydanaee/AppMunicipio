using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

[QueryProperty(nameof(Tipo), "tipo")]
[QueryProperty(nameof(ActivityId), "id")]
public partial class ActivityDetailPage : ContentPage
{
    private const string PhonePreferenceKey = "workshop_contact_phone";

    private Taller? _workshop;
    private Feria? _fair;
    private Campania? _campaign;
    private WorkshopRegistration? _registration;
    private bool _isBusy;

    public string Tipo { get; set; } = string.Empty;
    public string ActivityId { get; set; } = string.Empty;

    public ActivityDetailPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await LoadAsync();
        }
        catch (Exception ex)
        {
            SetLoading(false);
            await ShowUnexpectedErrorAsync(ex);
        }
    }

    private async Task LoadAsync()
    {
        ErrorBorder.IsVisible = false;
        SetLoading(true);

        try
        {
            if (!int.TryParse(ActivityId, out var id))
            {
                ShowError("No se recibió un identificador válido.");
                return;
            }

            if (string.Equals(Tipo, "Taller", StringComparison.OrdinalIgnoreCase))
            {
                var result = await AppServices.Api.GetAsync<Taller>($"/api/Taller/{id}");

                if (!result.IsSuccess || result.Data is null)
                {
                    ShowError(result.Error);
                    return;
                }

                ShowWorkshop(result.Data);
                await LoadRegistrationStatusAsync();
            }
            else if (string.Equals(Tipo, "Campania", StringComparison.OrdinalIgnoreCase))
            {
                var result = await AppServices.Api.GetAsync<Campania>($"/api/Campania/{id}");

                if (!result.IsSuccess || result.Data is null)
                {
                    ShowError(result.Error);
                    return;
                }

                ShowCampaign(result.Data);
            }
            else
            {
                var result = await AppServices.Api.GetAsync<Feria>($"/api/Feria/{id}");

                if (!result.IsSuccess || result.Data is null)
                {
                    ShowError(result.Error);
                    return;
                }

                ShowFair(result.Data);
            }
        }
        catch (Exception ex)
        {
            ShowError($"No se pudo cargar la actividad: {ex.Message}");
        }
        finally
        {
            SetLoading(false);
            UpdateActionButton();
        }
    }

    private void ShowWorkshop(Taller item)
    {
        _workshop = item;
        _fair = null;
        _campaign = null;
        _registration = null;

        HeroBorder.BackgroundColor = Color.FromArgb("#0878D1");
        BadgeLabel.Text = string.IsNullOrWhiteSpace(item.Modalidad)
            ? "Taller municipal"
            : item.Modalidad;
        TitleLabel.Text = item.NombreTaller;
        DescriptionLabel.Text = item.DescripcionTaller;
        DateLabel.Text = item.FechaInicio == default
            ? "Fecha por confirmar"
            : item.FechaInicio.ToString("dd MMMM yyyy");
        TimeLabel.Text = FormatTimeRange(item.HoraInicio, item.HoraFin);
        LocationLabel.Text = FirstNotEmpty(
            item.UbicacionTaller,
            item.SectorTaller,
            "Ubicación por confirmar");
        CapacityLabel.Text = CapacityText(item);

        PhoneSection.IsVisible = true;
        MyRegistrationsButton.IsVisible = true;

        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            PhoneEntry.Text = Preferences.Default.Get(
                PhonePreferenceKey,
                string.Empty);
        }
    }

    private void ShowFair(Feria item)
    {
        _workshop = null;
        _fair = item;
        _campaign = null;
        _registration = null;

        HeroBorder.BackgroundColor = Color.FromArgb("#F46A0A");
        BadgeLabel.Text = "Feria municipal";
        TitleLabel.Text = item.NombreFeria;
        DescriptionLabel.Text = item.DescripcionFeria;
        DateLabel.Text = item.FechaInicio == default
            ? "Fecha por confirmar"
            : item.FechaInicio.ToString("dd MMMM yyyy");
        TimeLabel.Text = FormatTimeRange(item.HoraInicio, item.HoraFin);
        LocationLabel.Text = FirstNotEmpty(
            item.UbicacionFeria,
            item.SectorFeria,
            "Ubicación por confirmar");
        CapacityLabel.Text = "Entrada y condiciones según la organización";

        PhoneSection.IsVisible = false;
        MyRegistrationsButton.IsVisible = false;
    }

    private void ShowCampaign(Campania item)
    {
        _workshop = null;
        _fair = null;
        _campaign = item;
        _registration = null;

        HeroBorder.BackgroundColor = Color.FromArgb("#20A464");
        BadgeLabel.Text = FirstNotEmpty(item.TipoCampania, "Bienestar animal");
        TitleLabel.Text = item.NombreCampania;
        DescriptionLabel.Text = item.DescripcionCampania;
        DateLabel.Text = item.FechaInicio == default
            ? "Fecha por confirmar"
            : item.FechaInicio.ToString("dd MMMM yyyy");
        TimeLabel.Text = FormatTimeRange(item.HoraInicio, item.HoraFin);
        LocationLabel.Text = FirstNotEmpty(
            item.UbicacionCampania,
            "Ubicación por confirmar");
        CapacityLabel.Text = FirstNotEmpty(
            item.ResponsableCampania,
            "Campaña municipal");

        PhoneSection.IsVisible = false;
        MyRegistrationsButton.IsVisible = false;
    }

    private async Task LoadRegistrationStatusAsync()
    {
        var user = await AppServices.Session.LoadAsync();

        if (_workshop is null ||
            user is null ||
            string.IsNullOrWhiteSpace(user.Cedula))
        {
            return;
        }

        var result = await AppServices.Api.GetAsync<WorkshopRegistration>(
            $"/api/Taller/{_workshop.IdTaller}/Inscripcion/{Uri.EscapeDataString(user.Cedula)}");

        if (result.IsSuccess)
        {
            _registration = result.Data;
        }
        else if (result.StatusCode != 404)
        {
            ShowError(result.Error);
        }
    }

    private async void OnPrimaryActionClicked(object sender, EventArgs e)
    {
        if (_isBusy)
            return;

        try
        {
            ErrorBorder.IsVisible = false;

            if (_workshop is not null)
            {
                if (_registration is null)
                    await RegisterForWorkshopAsync();
                else
                    await CancelWorkshopRegistrationAsync();

                return;
            }

            if (_fair is not null)
            {
                await OpenMapAsync(FirstNotEmpty(
                    _fair.UbicacionFeria,
                    _fair.SectorFeria,
                    _fair.NombreFeria));
                return;
            }

            if (_campaign is not null)
            {
                await OpenMapAsync(FirstNotEmpty(
                    _campaign.UbicacionCampania,
                    _campaign.NombreCampania));
            }
        }
        catch (Exception ex)
        {
            SetLoading(false);
            await ShowUnexpectedErrorAsync(ex);
        }
    }

    private async Task RegisterForWorkshopAsync()
    {
        if (_workshop is null)
            return;

        var user = await AppServices.Session.LoadAsync();

        if (user is null)
        {
            var login = await DisplayAlert(
                "Inicio de sesión",
                "Debes iniciar sesión para reservar un cupo.",
                "Iniciar sesión",
                "Cancelar");

            if (login)
                await Shell.Current.GoToAsync(nameof(LoginPage));

            return;
        }

        if (string.IsNullOrWhiteSpace(user.Cedula) ||
            string.IsNullOrWhiteSpace(user.Email))
        {
            ShowError("La cuenta no tiene cédula o correo. Cierra sesión y vuelve a ingresar.");
            return;
        }

        var phone = PhoneEntry.Text?.Trim() ?? string.Empty;
        var digitCount = phone.Count(char.IsDigit);

        if (digitCount < 7)
        {
            ShowError("Escribe un teléfono de contacto de al menos 7 dígitos.");
            PhoneEntry.Focus();
            return;
        }

        Preferences.Default.Set(PhonePreferenceKey, phone);
        SetLoading(true);

        try
        {
            var result = await AppServices.Api.PostJsonAsync<
                TallerInscripcionRequest,
                WorkshopRegistration>(
                "/api/Taller/Inscribirse",
                new TallerInscripcionRequest
                {
                    IdTaller = _workshop.IdTaller,
                    Nombre = string.IsNullOrWhiteSpace(user.Nombre)
                        ? user.Email
                        : user.Nombre,
                    Cedula = user.Cedula,
                    Correo = user.Email,
                    Telefono = phone
                });

            if (!result.IsSuccess || result.Data is null)
            {
                ShowError(string.IsNullOrWhiteSpace(result.Error)
                    ? "La API no devolvió la inscripción creada."
                    : result.Error);
                return;
            }

            _registration = result.Data;
            _workshop.Inscritos++;
            _workshop.CuposDisponibles = Math.Max(
                0,
                _workshop.CuposTaller - _workshop.Inscritos);
            CapacityLabel.Text = CapacityText(_workshop);
            UpdateActionButton();

            await DisplayAlert(
                "Cupo reservado",
                "Tu inscripción fue registrada correctamente.",
                "Aceptar");
        }
        finally
        {
            SetLoading(false);
            UpdateActionButton();
        }
    }

    private async Task CancelWorkshopRegistrationAsync()
    {
        if (_workshop is null || _registration is null)
            return;

        var confirm = await DisplayAlert(
            "Cancelar inscripción",
            $"¿Deseas liberar tu cupo en “{_workshop.NombreTaller}”?",
            "Sí, cancelar",
            "Volver");

        if (!confirm)
            return;

        SetLoading(true);

        try
        {
            var result = await AppServices.Api.DeleteAsync(
                $"/api/Taller/Cancelar/{_registration.IdInscripcion}");

            if (!result.IsSuccess)
            {
                ShowError(result.Error);
                return;
            }

            _registration = null;
            _workshop.Inscritos = Math.Max(0, _workshop.Inscritos - 1);
            _workshop.CuposDisponibles = Math.Max(
                0,
                _workshop.CuposTaller - _workshop.Inscritos);
            CapacityLabel.Text = CapacityText(_workshop);
            UpdateActionButton();

            await DisplayAlert(
                "Inscripción cancelada",
                "El cupo fue liberado correctamente.",
                "Aceptar");
        }
        finally
        {
            SetLoading(false);
            UpdateActionButton();
        }
    }

    private async void OnMyRegistrationsClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(MyRegistrationsPage));
        }
        catch (Exception ex)
        {
            await ShowUnexpectedErrorAsync(ex);
        }
    }

    private async void OnShareClicked(object sender, EventArgs e)
    {
        try
        {
            var title = _workshop?.NombreTaller
                ?? _fair?.NombreFeria
                ?? _campaign?.NombreCampania
                ?? "Actividad municipal";

            var location = _workshop is not null
                ? FirstNotEmpty(_workshop.UbicacionTaller, _workshop.SectorTaller)
                : _fair is not null
                    ? FirstNotEmpty(_fair.UbicacionFeria, _fair.SectorFeria)
                    : FirstNotEmpty(_campaign?.UbicacionCampania ?? string.Empty);

            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Title = title,
                Text = $"{title}\n{DateLabel.Text}\n{TimeLabel.Text}\n{location}\nMunicipio de Quito"
            });
        }
        catch (Exception ex)
        {
            await ShowUnexpectedErrorAsync(ex);
        }
    }

    private static async Task OpenMapAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;

        try
        {
            await Launcher.Default.OpenAsync(new Uri(
                $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(query)}"));
        }
        catch
        {
            // El equipo puede no tener una aplicación o navegador disponible.
        }
    }

    private void UpdateActionButton()
    {
        if (_workshop is not null)
        {
            var available = Available(_workshop);

            if (_registration is not null)
            {
                PrimaryActionButton.Text = "Cancelar mi cupo";
                PrimaryActionButton.BackgroundColor = Color.FromArgb("#E42319");
                PrimaryActionButton.IsEnabled = !_isBusy;
            }
            else if (_workshop.CuposTaller > 0 && available <= 0)
            {
                PrimaryActionButton.Text = "Sin cupos disponibles";
                PrimaryActionButton.BackgroundColor = Color.FromArgb("#7B8794");
                PrimaryActionButton.IsEnabled = false;
            }
            else
            {
                PrimaryActionButton.Text = "Reservar cupo";
                PrimaryActionButton.BackgroundColor = Color.FromArgb("#0878D1");
                PrimaryActionButton.IsEnabled = !_isBusy;
            }
        }
        else if (_fair is not null)
        {
            PrimaryActionButton.Text = "Ver ubicación";
            PrimaryActionButton.BackgroundColor = Color.FromArgb("#F46A0A");
            PrimaryActionButton.IsEnabled = !_isBusy;
        }
        else if (_campaign is not null)
        {
            PrimaryActionButton.Text = "Ver ubicación";
            PrimaryActionButton.BackgroundColor = Color.FromArgb("#20A464");
            PrimaryActionButton.IsEnabled = !_isBusy;
        }
    }

    private void ShowError(string? error)
    {
        ErrorLabel.Text = string.IsNullOrWhiteSpace(error)
            ? "No se pudo completar la operación."
            : error.Trim('"');
        ErrorBorder.IsVisible = true;
    }

    private async Task ShowUnexpectedErrorAsync(Exception ex)
    {
        var message = string.IsNullOrWhiteSpace(ex.Message)
            ? "Ocurrió un error inesperado."
            : ex.Message;

        ShowError(message);

        try
        {
            await DisplayAlert(
                "No se pudo completar la operación",
                message,
                "Aceptar");
        }
        catch
        {
            // El error ya quedó visible dentro de la página.
        }
    }

    private void SetLoading(bool value)
    {
        _isBusy = value;
        LoadingIndicator.IsVisible = value;
        LoadingIndicator.IsRunning = value;

        if (value)
            PrimaryActionButton.IsEnabled = false;
        else
            UpdateActionButton();
    }

    private static int Available(Taller item)
    {
        if (item.CuposTaller <= 0)
            return 0;

        return Math.Max(0, item.CuposTaller - item.Inscritos);
    }

    private static string CapacityText(Taller item)
    {
        return item.CuposTaller > 0
            ? $"{Available(item)} cupos disponibles de {item.CuposTaller}"
            : "Cupos por confirmar";
    }

    private static string FormatTimeRange(string? start, string? end)
    {
        static string TrimTime(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Length >= 5 ? value[..5] : value;
        }

        var first = TrimTime(start);
        var second = TrimTime(end);

        return string.IsNullOrWhiteSpace(first)
            ? "Horario por confirmar"
            : string.IsNullOrWhiteSpace(second)
                ? first
                : $"{first} - {second}";
    }

    private static string FirstNotEmpty(params string[] values)
    {
        return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))
            ?? string.Empty;
    }
}
