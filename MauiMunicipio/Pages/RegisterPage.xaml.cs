using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        var name = NameEntry.Text?.Trim() ?? string.Empty;
        var cedula = IdEntry.Text?.Trim() ?? string.Empty;
        var email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        var sector = SectorEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;
        var confirm = ConfirmEntry.Text ?? string.Empty;

        if (new[] { name, cedula, email, sector, password, confirm }.Any(string.IsNullOrWhiteSpace))
        {
            ShowError("Completa todos los campos.");
            return;
        }
        if (cedula.Length != 10 || !cedula.All(char.IsDigit))
        {
            ShowError("La cédula debe tener 10 dígitos.");
            return;
        }
        if (!email.Contains('@') || !email.Contains('.'))
        {
            ShowError("Ingresa un correo válido.");
            return;
        }
        if (password.Length < 6)
        {
            ShowError("La contraseña debe tener al menos 6 caracteres.");
            return;
        }
        if (password != confirm)
        {
            ShowError("Las contraseñas no coinciden.");
            return;
        }

        SetLoading(true);
        var result = await AppServices.Api.PostJsonAsync<RegisterRequest, LoginResponse>(
            "/api/Ingreso/registro",
            new RegisterRequest
            {
                NombreCompleto = name,
                Cedula = cedula,
                Email = email,
                Sector = sector,
                Password = password
            });
        SetLoading(false);

        if (!result.IsSuccess || result.Data is null)
        {
            ShowError(string.IsNullOrWhiteSpace(result.Error) ? "No se pudo completar el registro." : result.Error);
            return;
        }

        await AppServices.Session.SaveAsync(new UserSession
        {
            Id = result.Data.Id ?? string.Empty,
            Nombre = result.Data.NombreCompleto ?? result.Data.Nombre ?? name,
            Email = result.Data.Email ?? email,
            Cedula = result.Data.Cedula ?? cedula,
            Sector = result.Data.Sector ?? sector,
            Roles = result.Data.Roles ?? []
        });

        await DisplayAlert("Cuenta creada", "Tu cuenta fue creada y la sesión quedó iniciada.", "Continuar");
        await Shell.Current.GoToAsync("//principal/inicio");
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
        RegisterButton.IsEnabled = !loading;
    }
}
