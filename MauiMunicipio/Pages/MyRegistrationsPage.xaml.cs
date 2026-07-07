using System.Collections.ObjectModel;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class MyRegistrationsPage : ContentPage
{
    public ObservableCollection<WorkshopRegistration> Registrations { get; } = [];

    public MyRegistrationsPage()
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
        var user = await AppServices.Session.LoadAsync();
        Registrations.Clear();

        if (user is null)
        {
            var login = await DisplayAlert("Inicio de sesión", "Inicia sesión para consultar tus inscripciones.", "Iniciar sesión", "Cancelar");
            if (login)
                await Shell.Current.GoToAsync(nameof(LoginPage));
            return;
        }

        SetLoading(true);
        var result = await AppServices.Api.GetListAsync<WorkshopRegistration>(
            $"/api/Taller/MisInscripciones/{Uri.EscapeDataString(user.Cedula)}");
        SetLoading(false);

        if (!result.IsSuccess)
        {
            await DisplayAlert("Mis inscripciones", result.Error, "Aceptar");
            return;
        }

        foreach (var registration in result.Data ?? [])
            Registrations.Add(registration);
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: WorkshopRegistration item })
            return;

        var confirm = await DisplayAlert(
            "Cancelar cupo",
            $"¿Deseas cancelar tu inscripción en “{item.NombreTaller}”?",
            "Sí, cancelar",
            "Volver");
        if (!confirm)
            return;

        SetLoading(true);
        var result = await AppServices.Api.DeleteAsync($"/api/Taller/Cancelar/{item.IdInscripcion}");
        SetLoading(false);

        if (!result.IsSuccess)
        {
            await DisplayAlert("No se pudo cancelar", result.Error, "Aceptar");
            return;
        }

        Registrations.Remove(item);
        await DisplayAlert("Inscripción cancelada", "El cupo fue liberado correctamente.", "Aceptar");
    }

    private async void OnDetailClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: WorkshopRegistration item })
            return;
        await Shell.Current.GoToAsync($"{nameof(ActivityDetailPage)}?tipo=Taller&id={item.IdTaller}");
    }

    private async void OnExploreClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//principal/participacion");
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
}
