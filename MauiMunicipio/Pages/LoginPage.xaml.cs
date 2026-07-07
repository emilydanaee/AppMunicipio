using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e) => await LoginAsync();
    private async void OnLoginCompleted(object sender, EventArgs e) => await LoginAsync();

    private async Task LoginAsync()
    {
        ErrorLabel.IsVisible = false;
        var email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Ingresa tu correo y contraseña.");
            return;
        }

        SetLoading(true);
        var result = await AppServices.Api.PostJsonAsync<LoginRequest, LoginResponse>(
            "/api/Ingreso/login",
            new LoginRequest { Email = email, Password = password });
        SetLoading(false);

        if (!result.IsSuccess || result.Data is null)
        {
            ShowError(string.IsNullOrWhiteSpace(result.Error) ? "No se pudo iniciar sesión." : result.Error);
            return;
        }

        await SaveSessionAsync(result.Data, email);
        PasswordEntry.Text = string.Empty;
        await Shell.Current.GoToAsync("//principal/inicio");
    }

    private static async Task SaveSessionAsync(LoginResponse response, string fallbackEmail)
    {
        await AppServices.Session.SaveAsync(new UserSession
        {
            Id = response.Id ?? string.Empty,
            Nombre = response.NombreCompleto ?? response.Nombre ?? fallbackEmail,
            Email = response.Email ?? fallbackEmail,
            Cedula = response.Cedula ?? string.Empty,
            Sector = response.Sector ?? string.Empty,
            Roles = response.Roles ?? []
        });
    }

    private void ShowError(string text)
    {
        ErrorLabel.Text = text.Trim('"');
        ErrorLabel.IsVisible = true;
    }

    private void SetLoading(bool loading)
    {
        LoadingIndicator.IsVisible = loading;
        LoadingIndicator.IsRunning = loading;
        LoginButton.IsEnabled = !loading;
        EmailEntry.IsEnabled = !loading;
        PasswordEntry.IsEnabled = !loading;
    }

    private void OnTogglePassword(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnApiSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ApiSettingsPage));
    }
}
