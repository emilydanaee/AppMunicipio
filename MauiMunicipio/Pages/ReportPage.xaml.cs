using System.Globalization;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class ReportPage : ContentPage
{
    private Location? _location;
    private FileResult? _photo;

    public ReportPage()
    {
        InitializeComponent();
        TypePicker.ItemsSource = new[]
        {
            "Bache o daño en vía",
            "Alumbrado público",
            "Basura o limpieza",
            "Ruido",
            "Seguridad ciudadana",
            "Persona en situación de calle",
            "Violencia intrafamiliar",
            "Violencia de género",
            "Acoso en espacio público",
            "Otro"
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var session = await AppServices.Session.LoadAsync();
        if (session is not null && string.IsNullOrWhiteSpace(ZonalEntry.Text))
            ZonalEntry.Text = session.Sector;
    }

    private async void OnGpsClicked(object sender, EventArgs e)
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert(
                    "Permiso requerido",
                    "Activa el permiso de ubicación en la configuración del dispositivo para usar el GPS.",
                    "Aceptar");
                return;
            }

            GpsButton.IsEnabled = false;
            LocationLabel.Text = "Obteniendo ubicación GPS...";

            var lastKnown = await Geolocation.Default.GetLastKnownLocationAsync();
            try
            {
                _location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(
                    GeolocationAccuracy.Best,
                    TimeSpan.FromSeconds(18)));
            }
            catch
            {
                _location = lastKnown;
            }

            if (_location is null)
            {
                LocationLabel.Text = "No fue posible obtener la ubicación.";
                await DisplayAlert("GPS", "Enciende la ubicación de Windows o del teléfono e inténtalo nuevamente.", "Aceptar");
                return;
            }

            LocationLabel.Text = $"✓ Latitud: {_location.Latitude:F6} · Longitud: {_location.Longitude:F6}";
            LocationLabel.TextColor = Color.FromArgb("#168A52");
            await TryFillAddressAsync(_location);
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("GPS no compatible", "Este dispositivo no ofrece ubicación GPS.", "Aceptar");
        }
        catch (Exception ex)
        {
            LocationLabel.Text = "Ubicación no disponible.";
            await DisplayAlert("GPS", $"No se pudo obtener la ubicación: {ex.Message}", "Aceptar");
        }
        finally
        {
            GpsButton.IsEnabled = true;
        }
    }

    private async Task TryFillAddressAsync(Location location)
    {
        if (!string.IsNullOrWhiteSpace(AddressEntry.Text))
            return;

        try
        {
            var placemark = (await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude))
                .FirstOrDefault();
            if (placemark is null)
                return;

            var street = string.Join(" ", new[] { placemark.SubThoroughfare, placemark.Thoroughfare }
                .Where(value => !string.IsNullOrWhiteSpace(value)));
            AddressEntry.Text = FirstNotEmpty(street, placemark.FeatureName, placemark.Locality, "Ubicación GPS adjunta");
            ParishEntry.Text = FirstNotEmpty(ParishEntry.Text ?? string.Empty, placemark.SubLocality, placemark.Locality);
        }
        catch
        {
            // Las coordenadas siguen siendo válidas aunque la geocodificación inversa no responda.
        }
    }

    private async void OnPhotoClicked(object sender, EventArgs e)
    {
        var option = await DisplayActionSheet(
            "Adjuntar evidencia",
            "Cancelar",
            null,
            "Tomar fotografía",
            "Elegir de la galería");

        if (option == "Cancelar" || string.IsNullOrWhiteSpace(option))
            return;

        try
        {
            if (option == "Tomar fotografía")
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await DisplayAlert("Cámara", "La captura de fotografías no está disponible en este dispositivo.", "Aceptar");
                    return;
                }

                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso requerido", "Activa el permiso de cámara para tomar la fotografía.", "Aceptar");
                    return;
                }

                _photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Evidencia del reporte"
                });
            }
            else
            {
                _photo = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Selecciona una fotografía del reporte"
                });
            }

            PhotoLabel.Text = _photo?.FileName ?? "No se ha seleccionado una imagen";
            PhotoLabel.TextColor = _photo is null
                ? Color.FromArgb("#5E6B78")
                : Color.FromArgb("#168A52");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fotografía", $"No se pudo adjuntar la imagen: {ex.Message}", "Aceptar");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        var type = TypePicker.SelectedItem?.ToString() ?? string.Empty;
        var description = DescriptionEditor.Text?.Trim() ?? string.Empty;
        var address = AddressEntry.Text?.Trim() ?? string.Empty;
        var zonal = ZonalEntry.Text?.Trim() ?? string.Empty;
        var parish = ParishEntry.Text?.Trim() ?? string.Empty;
        var phone = PhoneEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(description))
        {
            ShowError("Selecciona el tipo de caso y escribe una descripción.");
            return;
        }
        if (description.Length < 10)
        {
            ShowError("Describe el caso con al menos 10 caracteres.");
            return;
        }
        if (string.IsNullOrWhiteSpace(address) && _location is null)
        {
            ShowError("Ingresa una dirección o captura la ubicación con GPS.");
            return;
        }

        var session = await AppServices.Session.LoadAsync();
        var anonymous = AnonymousSwitch.IsToggled;
        if (!anonymous && session is null)
        {
            var login = await DisplayAlert(
                "Inicio de sesión",
                "Inicia sesión para enviar el reporte con tus datos o activa la opción anónima.",
                "Iniciar sesión",
                "Cancelar");
            if (login)
                await Shell.Current.GoToAsync(nameof(LoginPage));
            return;
        }

        var fields = new Dictionary<string, string?>
        {
            ["TipoReporte"] = type,
            ["DescripcionReporte"] = description,
            ["AdministracionZonal"] = zonal,
            ["Parroquia"] = parish,
            ["Latitud"] = (_location?.Latitude ?? 0).ToString(CultureInfo.InvariantCulture),
            ["Longitud"] = (_location?.Longitude ?? 0).ToString(CultureInfo.InvariantCulture),
            ["Direccion"] = address,
            ["Cedula"] = anonymous ? "ANONIMO" : session!.Cedula,
            ["Correo"] = anonymous ? "anonimo@municipio.local" : session!.Email,
            ["Telefono"] = anonymous ? string.Empty : phone
        };

        SetLoading(true);
        var result = await AppServices.Api.PostMultipartAsync<MauiMunicipio.Models.Reporte>(
            "/api/Reporte",
            fields,
            _photo,
            "Imagen");
        SetLoading(false);

        if (!result.IsSuccess)
        {
            ShowError(result.Error);
            return;
        }

        var ticket = result.Data?.IdReporte > 0 ? $" Número de seguimiento: {result.Data.IdReporte}." : string.Empty;
        await DisplayAlert(
            "Reporte enviado",
            $"El reporte fue registrado correctamente.{ticket}",
            "Aceptar");
        ClearForm();
    }

    private async void OnMyReportsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyReportsPage));
    }

    private void OnClearClicked(object sender, EventArgs e) => ClearForm();

    private void ClearForm()
    {
        TypePicker.SelectedIndex = -1;
        DescriptionEditor.Text = string.Empty;
        AddressEntry.Text = string.Empty;
        ParishEntry.Text = string.Empty;
        PhoneEntry.Text = string.Empty;
        AnonymousSwitch.IsToggled = false;
        _location = null;
        _photo = null;
        LocationLabel.Text = "Ubicación no capturada";
        LocationLabel.TextColor = Color.FromArgb("#5E6B78");
        PhotoLabel.Text = "No se ha seleccionado una imagen";
        PhotoLabel.TextColor = Color.FromArgb("#5E6B78");
        ErrorLabel.IsVisible = false;
    }

    private void ShowError(string text)
    {
        ErrorLabel.Text = string.IsNullOrWhiteSpace(text) ? "No se pudo enviar el reporte." : text.Trim('"');
        ErrorLabel.IsVisible = true;
    }

    private void SetLoading(bool value)
    {
        LoadingIndicator.IsVisible = value;
        LoadingIndicator.IsRunning = value;
        SubmitButton.IsEnabled = !value;
        GpsButton.IsEnabled = !value;
        PhotoButton.IsEnabled = !value;
    }

    private static string FirstNotEmpty(params string[] values) =>
        values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;
}
