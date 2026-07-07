using System.Collections.ObjectModel;
using System.Globalization;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class RequestsPage : ContentPage
{
    private string _section = "Permisos";
    public ObservableCollection<RequestCard> Items { get; } = [];

    public RequestsPage()
    {
        InitializeComponent();
        BindingContext = this;
        UpdateTabs();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        Items.Clear();
        SetLoading(true);

        var session = await AppServices.Session.LoadAsync();
        if (_section is "Permisos" or "Reservas")
        {
            if (session is null)
            {
                SetLoading(false);
                var login = await DisplayAlert(
                    "Inicio de sesión",
                    "Inicia sesión para registrar y consultar tus solicitudes.",
                    "Iniciar sesión",
                    "Cancelar");
                if (login)
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                return;
            }
        }

        if (_section == "Permisos")
            await LoadPermitsAsync(session!);
        else if (_section == "Reservas")
            await LoadReservationsAsync(session!);
        else
            await LoadTaxesAsync(session?.Cedula);

        SetLoading(false);
    }

    private async Task LoadPermitsAsync(UserSession session)
    {
        var result = await AppServices.Api.GetListAsync<Permiso>(
            $"/api/Permiso/usuario/{Uri.EscapeDataString(session.Cedula)}");
        if (!result.IsSuccess)
        {
            await ShowError(result.Error);
            return;
        }

        foreach (var item in result.Data ?? [])
        {
            Items.Add(new RequestCard
            {
                Id = item.IdPermiso,
                Icon = "📄",
                Type = "Permiso",
                Title = item.TipoPermiso,
                Description = FirstNotEmpty(item.DescripcionPermiso, "Solicitud municipal"),
                DateText = item.FechaPermiso == default ? "Sin fecha" : item.FechaPermiso.ToString("dd MMM yyyy"),
                Status = FirstNotEmpty(item.EstadoPermiso, "Ingresado"),
                StatusColor = StatusColor(item.EstadoPermiso),
                CanCancel = CanCancel(item.EstadoPermiso)
            });
        }
    }

    private async Task LoadReservationsAsync(UserSession session)
    {
        var result = await AppServices.Api.GetListAsync<Reserva>(
            $"/api/Reserva/usuario/{Uri.EscapeDataString(session.Cedula)}");
        if (!result.IsSuccess)
        {
            await ShowError(result.Error);
            return;
        }

        foreach (var item in result.Data ?? [])
        {
            Items.Add(new RequestCard
            {
                Id = item.IdReserva,
                Icon = "🏛",
                Type = "Reserva",
                Title = item.EspacioReserva,
                Description = item.MotivoReserva,
                DateText = $"{FormatDate(item.FechaReserva)} · {ShortTime(item.HoraInicio)} - {ShortTime(item.HoraFin)}",
                Status = FirstNotEmpty(item.EstadoReserva, "Solicitada"),
                StatusColor = StatusColor(item.EstadoReserva),
                CanCancel = CanCancel(item.EstadoReserva)
            });
        }
    }

    private async Task LoadTaxesAsync(string? currentCedula)
    {
        var cedula = currentCedula;
        if (string.IsNullOrWhiteSpace(cedula))
        {
            cedula = await DisplayPromptAsync(
                "Consulta de impuestos",
                "Ingresa tu número de cédula:",
                keyboard: Keyboard.Numeric,
                maxLength: 10);
        }

        if (string.IsNullOrWhiteSpace(cedula))
            return;
        if (cedula.Length != 10 || !cedula.All(char.IsDigit))
        {
            await DisplayAlert("Cédula inválida", "La cédula debe tener 10 dígitos.", "Aceptar");
            return;
        }

        var result = await AppServices.Api.GetListAsync<Consulta>(
            $"/api/Consulta/cedula/{Uri.EscapeDataString(cedula)}");
        if (!result.IsSuccess)
        {
            await ShowError(result.Error);
            return;
        }

        foreach (var item in result.Data ?? [])
        {
            Items.Add(new RequestCard
            {
                Id = item.IdConsulta,
                Icon = "💲",
                Type = "Impuesto",
                Title = item.Pagado ? "Obligación pagada" : "Valor pendiente",
                Description = item.ValorPendiente.ToString("C2", CultureInfo.GetCultureInfo("es-EC")),
                DateText = item.FechaConsulta.ToString("dd MMM yyyy"),
                Status = item.Pagado ? "Pagado" : "Pendiente",
                StatusColor = item.Pagado ? "#20A464" : "#F46A0A",
                CanCancel = false
            });
        }

        if (Items.Count == 0)
        {
            await DisplayAlert(
                "Consulta completada",
                "No se encontraron obligaciones registradas para esa cédula.",
                "Aceptar");
        }
    }

    private async void OnNewClicked(object sender, EventArgs e)
    {
        if (_section == "Permisos")
            await Shell.Current.GoToAsync($"{nameof(UniversalFormPage)}?module=permiso&id=0");
        else if (_section == "Reservas")
            await Shell.Current.GoToAsync($"{nameof(UniversalFormPage)}?module=reserva&id=0");
        else
            await Shell.Current.GoToAsync($"{nameof(ManagementPage)}?module=consulta");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: RequestCard item })
            return;

        var confirm = await DisplayAlert(
            "Cancelar solicitud",
            $"¿Deseas cancelar “{item.Title}”?",
            "Sí, cancelar",
            "Volver");
        if (!confirm)
            return;

        SetLoading(true);
        var endpoint = item.Type == "Permiso"
            ? $"/api/Permiso/{item.Id}"
            : $"/api/Reserva/{item.Id}";
        var result = await AppServices.Api.DeleteAsync(endpoint);
        SetLoading(false);

        if (!result.IsSuccess)
        {
            await ShowError(result.Error);
            return;
        }

        Items.Remove(item);
        await DisplayAlert("Solicitud cancelada", "La solicitud fue cancelada correctamente.", "Aceptar");
    }

    private async void OnPermitsClicked(object sender, EventArgs e)
    {
        _section = "Permisos";
        UpdateTabs();
        await LoadAsync();
    }

    private async void OnReservationsClicked(object sender, EventArgs e)
    {
        _section = "Reservas";
        UpdateTabs();
        await LoadAsync();
    }

    private async void OnTaxesClicked(object sender, EventArgs e)
    {
        _section = "Impuestos";
        UpdateTabs();
        await LoadAsync();
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadAsync();
        RefreshControl.IsRefreshing = false;
    }

    private void UpdateTabs()
    {
        SetTab(PermitsButton, _section == "Permisos");
        SetTab(ReservationsButton, _section == "Reservas");
        SetTab(TaxesButton, _section == "Impuestos");
        NewButton.Text = _section == "Impuestos" ? "⌕ Consultar" : "＋ Nueva";
        EmptyTitle.Text = _section == "Impuestos" ? "No hay obligaciones registradas" : "No hay solicitudes";
        EmptySubtitle.Text = _section == "Impuestos"
            ? "Presiona Consultar para volver a buscar."
            : "Usa el botón Nueva para registrar una solicitud.";
    }

    private static void SetTab(Button button, bool selected)
    {
        button.BackgroundColor = selected ? Color.FromArgb("#0878D1") : Colors.White;
        button.TextColor = selected ? Colors.White : Color.FromArgb("#5E6B78");
        button.BorderColor = Color.FromArgb(selected ? "#0878D1" : "#E2E8F0");
    }

    private void SetLoading(bool loading)
    {
        LoadingIndicator.IsVisible = loading;
        LoadingIndicator.IsRunning = loading;
        NewButton.IsEnabled = !loading;
    }

    private async Task ShowError(string error)
    {
        if (!string.IsNullOrWhiteSpace(error))
            await DisplayAlert("Solicitudes", error.Trim('"'), "Aceptar");
    }

    private static bool CanCancel(string? status)
    {
        var value = status?.ToLowerInvariant() ?? string.Empty;
        return value.Contains("ingres") || value.Contains("pend") || value.Contains("solicit");
    }

    private static string FormatDate(string value) =>
        DateTime.TryParse(value, out var date) ? date.ToString("dd MMM yyyy") : value;

    private static string FirstNotEmpty(params string[] values) =>
        values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;

    private static string ShortTime(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "--:--" : value.Length >= 5 ? value[..5] : value;

    private static string StatusColor(string? status)
    {
        var value = status?.ToLowerInvariant() ?? string.Empty;
        if (value.Contains("aprobad") || value.Contains("pagad") || value.Contains("complet")) return "#20A464";
        if (value.Contains("rechaz") || value.Contains("cancel")) return "#E42319";
        if (value.Contains("pend") || value.Contains("solicit") || value.Contains("ingres")) return "#F46A0A";
        return "#0878D1";
    }
}
