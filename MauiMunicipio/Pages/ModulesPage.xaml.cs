using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class ModulesPage : ContentPage
{
    public ModulesPage()
    {
        InitializeComponent();
        AppServices.Session.SessionChanged += OnSessionChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AppServices.Session.LoadAsync();
        UpdateRoleLabel();
    }

    private void OnSessionChanged(object? sender, EventArgs e) =>
        MainThread.BeginInvokeOnMainThread(UpdateRoleLabel);

    private void UpdateRoleLabel()
    {
        var session = AppServices.Session.Current;
        RoleLabel.Text = session is null
            ? "Modo visitante · inicia sesión para registrar solicitudes"
            : session.IsAdmin
                ? "Modo administrador · gestión completa habilitada"
                : $"Modo ciudadano · {session.Nombre}";
    }

    private async void OnManagementClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not string module)
            return;

        await Shell.Current.GoToAsync($"{nameof(ManagementPage)}?module={Uri.EscapeDataString(module)}");
    }

    private async void OnDirectClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not string action)
            return;

        switch (action)
        {
            case "agenda":
                await Shell.Current.GoToAsync("//principal/participacion");
                break;
            case "reportar":
                await Shell.Current.GoToAsync("//principal/reportar");
                break;
            case "solicitudes":
                await Shell.Current.GoToAsync(nameof(RequestsPage));
                break;
            case "estadisticas":
                await Shell.Current.GoToAsync(nameof(DashboardStatsPage));
                break;
        }
    }
}
