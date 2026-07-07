using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class ApiSettingsPage : ContentPage
{
    public ApiSettingsPage()
    {
        InitializeComponent();
        UrlEntry.Text = ApiSettings.BaseUrl;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ApiSettings.BaseUrl = UrlEntry.Text ?? string.Empty;
        UrlEntry.Text = ApiSettings.BaseUrl;
        await DisplayAlert("Configuración guardada", $"La aplicación usará: {ApiSettings.BaseUrl}", "Aceptar");
    }

    private async void OnTestClicked(object sender, EventArgs e)
    {
        ApiSettings.BaseUrl = UrlEntry.Text ?? string.Empty;
        UrlEntry.Text = ApiSettings.BaseUrl;
        SetLoading(true);
        var result = await AppServices.Api.GetAsync<HealthResponse>("/api/Health");
        SetLoading(false);

        ResultLabel.IsVisible = true;
        if (result.IsSuccess && result.Data is not null &&
            string.Equals(result.Data.Status, "ok", StringComparison.OrdinalIgnoreCase))
        {
            ResultLabel.Text = "✓ Conexión correcta. API y base de datos disponibles.";
            ResultLabel.TextColor = Color.FromArgb("#168A52");
        }
        else
        {
            ResultLabel.Text = $"✕ {result.Error}";
            ResultLabel.TextColor = Color.FromArgb("#C51E16");
        }
    }

    private async void OnDefaultClicked(object sender, EventArgs e)
    {
        ApiSettings.Reset();
        UrlEntry.Text = ApiSettings.BaseUrl;
        await DisplayAlert("Dirección restaurada", $"Se usará: {ApiSettings.BaseUrl}", "Aceptar");
    }

    private void SetLoading(bool value)
    {
        LoadingIndicator.IsVisible = value;
        LoadingIndicator.IsRunning = value;
        TestButton.IsEnabled = !value;
    }
}
