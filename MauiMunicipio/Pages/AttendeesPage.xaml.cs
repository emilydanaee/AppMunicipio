using System.Collections.ObjectModel;
using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class AttendeesPage : ContentPage, IQueryAttributable
{
    private int _id;
    public ObservableCollection<WorkshopAttendee> Items { get; } = new();

    public AttendeesPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idValue))
            int.TryParse(idValue?.ToString(), out _id);
        if (query.TryGetValue("title", out var titleValue))
            WorkshopLabel.Text = Uri.UnescapeDataString(titleValue?.ToString() ?? "Taller");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadingIndicator.IsVisible = LoadingIndicator.IsRunning = true;
        Items.Clear();
        var result = await AppServices.Api.GetListAsync<WorkshopAttendee>($"/api/Taller/{_id}/Inscritos");
        LoadingIndicator.IsVisible = LoadingIndicator.IsRunning = false;
        if (!result.IsSuccess)
        {
            await DisplayAlert("Inscritos", result.Error.Trim('"'), "Aceptar");
            return;
        }
        foreach (var item in result.Data ?? new List<WorkshopAttendee>())
            Items.Add(item);
    }
}
