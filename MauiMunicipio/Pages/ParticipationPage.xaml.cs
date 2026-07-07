using System.Collections.ObjectModel;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class ParticipationPage : ContentPage
{
    private string _currentFilter = "Eventos";
    public ObservableCollection<ActivityCard> Activities { get; } = new();

    public ParticipationPage()
    {
        InitializeComponent();
        BindingContext = this;
        UpdateTabs();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadActivitiesAsync();
    }

    private async Task LoadActivitiesAsync()
    {
        RefreshControl.IsRefreshing = true;
        Activities.Clear();
        var errors = new List<string>();

        if (_currentFilter is "Eventos" or "Talleres")
        {
            var workshops = await AppServices.Api.GetListAsync<Taller>("/api/Taller");
            if (workshops.IsSuccess)
            {
                foreach (var item in (workshops.Data ?? new List<Taller>()).OrderBy(x => x.FechaInicio))
                    Activities.Add(MapWorkshop(item));
            }
            else
            {
                errors.Add(workshops.Error);
            }
        }

        if (_currentFilter is "Eventos" or "Ferias")
        {
            var fairs = await AppServices.Api.GetListAsync<Feria>("/api/Feria");
            if (fairs.IsSuccess)
            {
                foreach (var item in (fairs.Data ?? new List<Feria>()).OrderBy(x => x.FechaInicio))
                    Activities.Add(MapFair(item));
            }
            else
            {
                errors.Add(fairs.Error);
            }
        }

        if (_currentFilter is "Eventos" or "Bienestar")
        {
            var campaigns = await AppServices.Api.GetListAsync<Campania>("/api/Campania");
            if (campaigns.IsSuccess)
            {
                foreach (var item in (campaigns.Data ?? new List<Campania>()).OrderBy(x => x.FechaInicio))
                    Activities.Add(MapCampaign(item));
            }
            else
            {
                errors.Add(campaigns.Error);
            }
        }

        RefreshControl.IsRefreshing = false;

        if (Activities.Count == 0)
        {
            var error = errors.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
            if (!string.IsNullOrWhiteSpace(error))
                await ShowApiError(error);
        }
    }

    private ActivityCard MapWorkshop(Taller item)
    {
        var available = item.CuposDisponibles > 0
            ? item.CuposDisponibles
            : Math.Max(0, item.CuposTaller - item.Inscritos);

        return new ActivityCard
        {
            Id = item.IdTaller,
            Type = "Taller",
            Badge = string.IsNullOrWhiteSpace(item.Modalidad) ? "Taller" : item.Modalidad,
            BadgeColor = "#0878D1",
            AccentColor = "#0878D1",
            Title = item.NombreTaller,
            Description = item.DescripcionTaller,
            DateText = item.FechaInicio == default ? "Fecha por confirmar" : item.FechaInicio.ToString("dd MMM yyyy"),
            TimeText = FormatTimeRange(item.HoraInicio, item.HoraFin),
            LocationText = FirstNotEmpty(item.UbicacionTaller, item.SectorTaller, "Ubicación por confirmar"),
            CapacityText = item.CuposTaller > 0 ? $"{available}/{item.CuposTaller} cupos disponibles" : "Cupos por confirmar",
            ButtonText = available <= 0 && item.CuposTaller > 0 ? "Ver detalles" : "Reservar cupo",
            ImageUrl = AppServices.Api.BuildImageUrl(item.ImagenTaller)
        };
    }

    private ActivityCard MapFair(Feria item) => new()
    {
        Id = item.IdFeria,
        Type = "Feria",
        Badge = "Feria",
        BadgeColor = "#F46A0A",
        AccentColor = "#F46A0A",
        Title = item.NombreFeria,
        Description = item.DescripcionFeria,
        DateText = item.FechaInicio == default ? "Fecha por confirmar" : item.FechaInicio.ToString("dd MMM yyyy"),
        TimeText = FormatTimeRange(item.HoraInicio, item.HoraFin),
        LocationText = FirstNotEmpty(item.UbicacionFeria, item.SectorFeria, "Ubicación por confirmar"),
        CapacityText = "Entrada y condiciones según organización",
        ButtonText = "Ver detalles",
        ImageUrl = AppServices.Api.BuildImageUrl(item.ImagenFeria)
    };

    private ActivityCard MapCampaign(Campania item) => new()
    {
        Id = item.IdCampania,
        Type = "Campania",
        Badge = FirstNotEmpty(item.TipoCampania, "Bienestar animal"),
        BadgeColor = "#20A464",
        AccentColor = "#20A464",
        Title = item.NombreCampania,
        Description = item.DescripcionCampania,
        DateText = item.FechaInicio == default ? "Fecha por confirmar" : item.FechaInicio.ToString("dd MMM yyyy"),
        TimeText = FormatTimeRange(item.HoraInicio, item.HoraFin),
        LocationText = FirstNotEmpty(item.UbicacionCampania, "Ubicación por confirmar"),
        CapacityText = FirstNotEmpty(item.ResponsableCampania, "Campaña municipal"),
        ButtonText = "Ver detalles",
        ImageUrl = AppServices.Api.BuildImageUrl(item.ImagenCampania)
    };

    private static string FormatTimeRange(string? start, string? end)
    {
        var a = ShortTime(start);
        var b = ShortTime(end);
        if (string.IsNullOrWhiteSpace(a) && string.IsNullOrWhiteSpace(b))
            return "Horario por confirmar";
        return string.IsNullOrWhiteSpace(b) ? a : $"{a} - {b}";
    }

    private static string ShortTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        return value.Length >= 5 ? value[..5] : value;
    }

    private static string FirstNotEmpty(params string[] values) =>
        values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;

    private async Task ShowApiError(string error)
    {
        if (!string.IsNullOrWhiteSpace(error))
            await DisplayAlert("Conexión", error.Trim('"'), "Aceptar");
    }

    private async void OnRefreshing(object sender, EventArgs e) => await LoadActivitiesAsync();

    private async void OnEventsClicked(object sender, EventArgs e) => await SelectFilterAsync("Eventos");
    private async void OnWorkshopsClicked(object sender, EventArgs e) => await SelectFilterAsync("Talleres");
    private async void OnFairsClicked(object sender, EventArgs e) => await SelectFilterAsync("Ferias");
    private async void OnWellnessClicked(object sender, EventArgs e) => await SelectFilterAsync("Bienestar");

    private async Task SelectFilterAsync(string filter)
    {
        _currentFilter = filter;
        UpdateTabs();
        await LoadActivitiesAsync();
    }

    private void UpdateTabs()
    {
        SetTab(EventsButton, _currentFilter == "Eventos", "#9417F4");
        SetTab(WorkshopsButton, _currentFilter == "Talleres", "#0878D1");
        SetTab(FairsButton, _currentFilter == "Ferias", "#F46A0A");
        SetTab(WellnessButton, _currentFilter == "Bienestar", "#20A464");
    }

    private static void SetTab(Button button, bool selected, string color)
    {
        button.BackgroundColor = selected ? Color.FromArgb(color) : Colors.White;
        button.TextColor = selected ? Colors.White : Color.FromArgb("#5E6B78");
        button.BorderColor = Color.FromArgb(selected ? color : "#E2E8F0");
    }

    private async void OnActivityClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: ActivityCard item }) return;
        await Shell.Current.GoToAsync($"{nameof(ActivityDetailPage)}?tipo={Uri.EscapeDataString(item.Type)}&id={item.Id}");
    }

    private async void OnMyRegistrationsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync(nameof(MyRegistrationsPage));

    private async void OnCasaSomosTapped(object sender, TappedEventArgs e) => await SelectFilterAsync("Talleres");
    private async void OnFairsCategoryTapped(object sender, TappedEventArgs e) => await SelectFilterAsync("Ferias");
    private async void OnWellnessTapped(object sender, TappedEventArgs e) => await SelectFilterAsync("Bienestar");
}
