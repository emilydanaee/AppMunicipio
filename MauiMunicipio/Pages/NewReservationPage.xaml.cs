using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class NewReservationPage : ContentPage
{
    public NewReservationPage()
    {
        InitializeComponent();
        SpacePicker.ItemsSource = new[]
        {
            "Casa Somos",
            "Parque barrial",
            "Casa comunal",
            "Cancha deportiva",
            "Auditorio municipal",
            "Otro"
        };
        ReservationDatePicker.MinimumDate = DateTime.Today;
        ReservationDatePicker.Date = DateTime.Today.AddDays(7);
        StartTimePicker.Time = new TimeSpan(9, 0, 0);
        EndTimePicker.Time = new TimeSpan(11, 0, 0);
    }

    private void OnSpaceChanged(object sender, EventArgs e)
    {
        OtherSpaceEntry.IsVisible = string.Equals(
            SpacePicker.SelectedItem?.ToString(),
            "Otro",
            StringComparison.OrdinalIgnoreCase);
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        var user = await AppServices.Session.LoadAsync();
        if (user is null)
        {
            ShowError("Debes iniciar sesión para enviar la reserva.");
            return;
        }

        var selectedSpace = SpacePicker.SelectedItem?.ToString() ?? string.Empty;
        var space = selectedSpace == "Otro"
            ? OtherSpaceEntry.Text?.Trim() ?? string.Empty
            : selectedSpace;
        var motive = MotiveEditor.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(space) || motive.Length < 10)
        {
            ShowError("Selecciona el espacio y describe el motivo con al menos 10 caracteres.");
            return;
        }
        if (ReservationDatePicker.Date.Date < DateTime.Today)
        {
            ShowError("La fecha no puede estar en el pasado.");
            return;
        }
        if (EndTimePicker.Time <= StartTimePicker.Time)
        {
            ShowError("La hora final debe ser posterior a la hora inicial.");
            return;
        }

        var extraNotes = NotesEntry.Text?.Trim() ?? string.Empty;
        var metadata = $"[CEDULA:{user.Cedula}][CORREO:{user.Email}]";
        var request = new Reserva
        {
            EspacioReserva = space,
            FechaReserva = ReservationDatePicker.Date.ToString("yyyy-MM-dd"),
            HoraInicio = TimeOnly.FromTimeSpan(StartTimePicker.Time).ToString("HH:mm:ss"),
            HoraFin = TimeOnly.FromTimeSpan(EndTimePicker.Time).ToString("HH:mm:ss"),
            MotivoReserva = motive,
            EstadoReserva = "Solicitada",
            Observaciones = string.IsNullOrWhiteSpace(extraNotes) ? metadata : $"{metadata} {extraNotes}",
            DocumentoAdjunto = string.Empty
        };

        SetLoading(true);
        var result = await AppServices.Api.PostJsonAsync<Reserva, Reserva>("/api/Reserva", request);
        SetLoading(false);

        if (!result.IsSuccess)
        {
            ShowError(result.Error);
            return;
        }

        await DisplayAlert("Reserva enviada", "La solicitud de reserva fue registrada correctamente.", "Aceptar");
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
