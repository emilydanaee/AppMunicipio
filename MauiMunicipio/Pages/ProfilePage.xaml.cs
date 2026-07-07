using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AppServices.Session.LoadAsync();
        Render();
    }

    private void Render()
    {
        var user = AppServices.Session.Current;
        var loggedIn = user is not null;
        QuickActionsBorder.IsVisible = loggedIn;

        if (user is null)
        {
            InitialsLabel.Text = "I";
            NameLabel.Text = "Invitado";
            RoleLabel.Text = "Inicia sesión para acceder a todos los servicios";
            EmailLabel.Text = "Sin correo";
            IdLabel.Text = "Sin cédula";
            SectorLabel.Text = "Sin sector";
            AuthButton.Text = "Iniciar sesión";
            AuthButton.BackgroundColor = Color.FromArgb("#004B87");
            return;
        }

        InitialsLabel.Text = Initials(user.Nombre);
        NameLabel.Text = user.Nombre;
        RoleLabel.Text = user.IsAdmin ? "Administrador" : "Usuario ciudadano";
        EmailLabel.Text = string.IsNullOrWhiteSpace(user.Email) ? "Sin correo" : user.Email;
        IdLabel.Text = string.IsNullOrWhiteSpace(user.Cedula) ? "Sin cédula" : user.Cedula;
        SectorLabel.Text = string.IsNullOrWhiteSpace(user.Sector) ? "Sin sector" : user.Sector;
        AuthButton.Text = "Cerrar sesión";
        AuthButton.BackgroundColor = Color.FromArgb("#E42319");
    }

    private async void OnAuthClicked(object sender, EventArgs e)
    {
        if (!AppServices.Session.IsLoggedIn)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
            return;
        }

        var confirm = await DisplayAlert("Cerrar sesión", "¿Deseas cerrar tu sesión?", "Cerrar sesión", "Cancelar");
        if (!confirm)
            return;

        AppServices.Session.Clear();
        await Shell.Current.GoToAsync("//principal/inicio");
    }

    private async void OnRegistrationsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync(nameof(MyRegistrationsPage));

    private async void OnRequestsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync(nameof(RequestsPage));

    private async void OnReportsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync(nameof(MyReportsPage));

    private async void OnApiSettingsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync(nameof(ApiSettingsPage));

    private static string Initials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return "U";
        if (parts.Length == 1) return parts[0][0].ToString().ToUpperInvariant();
        return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
    }
}
