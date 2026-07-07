using System.Text.Json;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class NewPermitPage : ContentPage
{
    public NewPermitPage()
    {
        InitializeComponent();
        TypePicker.ItemsSource = new[]
        {
            "Permiso de funcionamiento",
            "Uso de espacio público",
            "Construcción",
            "Publicidad",
            "Actividad económica",
            "Otro"
        };
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        var user = await AppServices.Session.LoadAsync();
        if (user is null)
        {
            ShowError("Debes iniciar sesión para enviar la solicitud.");
            return;
        }

        var type = TypePicker.SelectedItem?.ToString() ?? string.Empty;
        var description = DescriptionEditor.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(type) || description.Length < 10)
        {
            ShowError("Selecciona el tipo y escribe una descripción de al menos 10 caracteres.");
            return;
        }
        if (!ConfirmSwitch.IsToggled)
        {
            ShowError("Confirma que la información ingresada es correcta.");
            return;
        }

        var request = new Permiso
        {
            TipoPermiso = type,
            DescripcionPermiso = description,
            FechaPermiso = DateTime.Now,
            EstadoPermiso = "Ingresado",
            DocumentoAdjunto = ReferenceEntry.Text?.Trim() ?? string.Empty,
            Observaciones = $"[CEDULA:{user.Cedula}][CORREO:{user.Email}]"
        };

        SetLoading(true);
        var result = await AppServices.Api.PostJsonAsync<Permiso, Permiso>("/api/Permiso", request);
        SetLoading(false);

        if (!result.IsSuccess)
        {
            ShowError(result.Error);
            return;
        }

        await DisplayAlert("Solicitud enviada", "El trámite fue registrado correctamente.", "Aceptar");
        await Shell.Current.GoToAsync("..");
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message.Trim('"');
        ErrorLabel.IsVisible = true;
    }

    private void SetLoading(bool loading)
    {
        LoadingIndicator.IsVisible = loading;
        LoadingIndicator.IsRunning = loading;
        SubmitButton.IsEnabled = !loading;
    }
}
