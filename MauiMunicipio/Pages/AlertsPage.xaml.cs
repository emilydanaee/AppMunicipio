using System.Collections.ObjectModel;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class AlertsPage : ContentPage
{
    public ObservableCollection<Alerta> Alerts { get; } = [];

    public AlertsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAlertsAsync();
    }

    private async Task LoadAlertsAsync()
    {
        RefreshControl.IsRefreshing = true;
        var result = await AppServices.Api.GetListAsync<Alerta>("/api/Alerta");
        Alerts.Clear();

        if (result.IsSuccess && result.Data is not null)
        {
            foreach (var alert in result.Data.OrderByDescending(item => item.Fecha))
            {
                if (string.IsNullOrWhiteSpace(alert.EstadoAlerta))
                    alert.EstadoAlerta = "Activa";
                Alerts.Add(alert);
            }
        }
        else if (!string.IsNullOrWhiteSpace(result.Error))
        {
            await DisplayAlert("Alertas", result.Error, "Aceptar");
        }

        RefreshControl.IsRefreshing = false;
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: Alerta alert })
            return;

        if (Math.Abs(alert.Latitud) > 0.000001 || Math.Abs(alert.Longitud) > 0.000001)
        {
            try
            {
                await Map.Default.OpenAsync(alert.Latitud, alert.Longitud, new MapLaunchOptions
                {
                    Name = alert.TipoAlerta,
                    NavigationMode = NavigationMode.None
                });
                return;
            }
            catch
            {
                // Se usa la dirección textual como alternativa.
            }
        }

        var query = string.IsNullOrWhiteSpace(alert.Direccion) ? alert.Sector : alert.Direccion;
        if (string.IsNullOrWhiteSpace(query))
        {
            await DisplayAlert("Ubicación", "Esta alerta no tiene una ubicación registrada.", "Aceptar");
            return;
        }

        await Launcher.Default.OpenAsync(new Uri(
            $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(query)}"));
    }

    private async void OnRefreshing(object sender, EventArgs e) => await LoadAlertsAsync();
    private async void OnRefreshClicked(object sender, EventArgs e) => await LoadAlertsAsync();
}
