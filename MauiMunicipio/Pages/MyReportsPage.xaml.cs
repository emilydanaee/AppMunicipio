using System.Collections.ObjectModel;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class MyReportsPage : ContentPage
{
    public ObservableCollection<ReportCard> Reports { get; } = [];

    public MyReportsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        Reports.Clear();
        var user = await AppServices.Session.LoadAsync();
        if (user is null)
        {
            var login = await DisplayAlert("Inicio de sesión", "Inicia sesión para consultar tus reportes.", "Iniciar sesión", "Cancelar");
            if (login)
                await Shell.Current.GoToAsync(nameof(LoginPage));
            return;
        }

        SetLoading(true);
        var result = await AppServices.Api.GetListAsync<Reporte>(
            $"/api/Reporte/usuario/{Uri.EscapeDataString(user.Cedula)}");
        SetLoading(false);

        if (!result.IsSuccess)
        {
            await DisplayAlert("Mis reportes", result.Error, "Aceptar");
            return;
        }

        foreach (var item in result.Data ?? [])
        {
            Reports.Add(new ReportCard
            {
                Id = item.IdReporte,
                Type = item.TipoReporte,
                Description = item.DescripcionReporte,
                DateText = item.FechaReporte.ToString("dd MMM yyyy HH:mm"),
                LocationText = FirstNotEmpty(item.Direccion, item.Parroquia, item.AdministracionZonal, "Ubicación GPS"),
                Status = FirstNotEmpty(item.EstadoReporte, "Pendiente"),
                StatusColor = StatusColor(item.EstadoReporte),
                Latitude = item.Latitud,
                Longitude = item.Longitud
            });
        }
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: ReportCard item } || !item.HasLocation)
            return;

        try
        {
            await Map.Default.OpenAsync(item.Latitude, item.Longitude, new MapLaunchOptions
            {
                Name = $"Reporte No. {item.Id}",
                NavigationMode = NavigationMode.None
            });
        }
        catch
        {
            await Launcher.Default.OpenAsync(new Uri(
                $"https://www.google.com/maps/search/?api=1&query={item.Latitude},{item.Longitude}"));
        }
    }

    private async void OnNewReportClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//principal/reportar");
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadAsync();
        RefreshControl.IsRefreshing = false;
    }

    private void SetLoading(bool loading)
    {
        LoadingIndicator.IsVisible = loading;
        LoadingIndicator.IsRunning = loading;
    }

    private static string StatusColor(string? status)
    {
        var value = status?.ToLowerInvariant() ?? string.Empty;
        if (value.Contains("resuelt") || value.Contains("cerrad") || value.Contains("complet")) return "#20A464";
        if (value.Contains("rechaz") || value.Contains("cancel")) return "#E42319";
        if (value.Contains("proceso") || value.Contains("atención")) return "#0878D1";
        return "#F46A0A";
    }

    private static string FirstNotEmpty(params string[] values) =>
        values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;
}
