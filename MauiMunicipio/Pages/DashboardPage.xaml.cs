using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        AppServices.Session.SessionChanged += OnSessionChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AppServices.Session.LoadAsync();
        UpdateSessionUi();
    }

    private void OnSessionChanged(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(UpdateSessionUi);
    }

    private void UpdateSessionUi()
    {
        var user = AppServices.Session.Current;
        WelcomeLabel.Text = user is null || string.IsNullOrWhiteSpace(user.Nombre)
            ? "Bienvenido"
            : $"Bienvenido, {FirstName(user.Nombre)}";
        SessionButton.Text = user is null ? "Iniciar sesión" : "Ver mi perfil";
    }

    private static string FirstName(string name)
    {
        return name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? name;
    }

    private async void OnSessionClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppServices.Session.IsLoggedIn
            ? nameof(ProfilePage)
            : nameof(LoginPage));
    }

    private async void OnRequestsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RequestsPage));
    }

    private async void OnReportTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//principal/reportar");
    }

    private async void OnParticipationTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//principal/participacion");
    }

    private async void OnSearchCompleted(object sender, EventArgs e)
    {
        var text = SearchEntry.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        if (text.Contains("reporte") || text.Contains("problema"))
            await Shell.Current.GoToAsync("//principal/reportar");
        else if (text.Contains("taller") || text.Contains("feria") || text.Contains("evento"))
            await Shell.Current.GoToAsync("//principal/participacion");
        else if (text.Contains("alerta"))
            await Shell.Current.GoToAsync("//principal/alertas");
        else if (text.Contains("trámite") || text.Contains("tramite") || text.Contains("solicitud") || text.Contains("reserva") || text.Contains("impuesto"))
            await Shell.Current.GoToAsync(nameof(RequestsPage));
        else if (text.Contains("obra") || text.Contains("violencia") || text.Contains("calle") || text.Contains("campaña") || text.Contains("campania") || text.Contains("módulo") || text.Contains("modulo"))
            await Shell.Current.GoToAsync("//principal/modulos");
        else
            await DisplayAlert("Búsqueda", "Escribe: trámites, solicitudes, talleres, ferias, reportes o alertas.", "Aceptar");
    }
    private async void OnAlertsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//principal/alertas");
    }

    private async void OnModulesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//principal/modulos");
    }

}
