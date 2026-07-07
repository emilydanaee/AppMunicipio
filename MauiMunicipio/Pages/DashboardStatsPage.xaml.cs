using MauiMunicipio.Models;
using MauiMunicipio.Services;

namespace MauiMunicipio.Pages;

public partial class DashboardStatsPage : ContentPage
{
    public DashboardStatsPage() => InitializeComponent();

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        LoadingIndicator.IsVisible = LoadingIndicator.IsRunning = true;
        try
        {
            var talleres = await AppServices.Api.GetListAsync<Taller>("/api/Taller");
            var ferias = await AppServices.Api.GetListAsync<Feria>("/api/Feria");
            var campanias = await AppServices.Api.GetListAsync<Campania>("/api/Campania");
            var inscripciones = await AppServices.Api.GetListAsync<InscripcionGeneral>("/api/Inscripcion");
            var reportes = await AppServices.Api.GetListAsync<Reporte>("/api/Reporte");
            var alertas = await AppServices.Api.GetListAsync<Alerta>("/api/Alerta");
            var calle = await AppServices.Api.GetListAsync<SituacionCalle>("/api/SituacionCalle");
            var violencia = await AppServices.Api.GetListAsync<ReporteViolencia>("/api/ReporteViolencia");
            var permisos = await AppServices.Api.GetListAsync<Permiso>("/api/Permiso");
            var reservas = await AppServices.Api.GetListAsync<Reserva>("/api/Reserva");
            var consultas = await AppServices.Api.GetListAsync<Consulta>("/api/Consulta");
            var obras = await AppServices.Api.GetListAsync<Obra>("/api/Obra");

            var activities = Count(talleres) + Count(ferias) + Count(campanias);
            var registrations = Count(inscripciones);
            var reports = reportes.Data ?? new List<Reporte>();
            var cases = Count(alertas) + Count(calle) + Count(violencia);
            var requests = Count(permisos) + Count(reservas) + Count(consultas);
            var works = Count(obras);

            ActivitiesLabel.Text = activities.ToString();
            RegistrationsLabel.Text = registrations.ToString();
            ReportsLabel.Text = reports.Count.ToString();
            CasesLabel.Text = cases.ToString();
            RequestsLabel.Text = requests.ToString();
            WorksLabel.Text = works.ToString();

            var pending = reports.Count(r => Contains(r.EstadoReporte, "pend", "recibid", "ingres"));
            var progress = reports.Count(r => Contains(r.EstadoReporte, "revis", "atenc", "seguim"));
            var closed = reports.Count(r => Contains(r.EstadoReporte, "cerr", "complet", "resuelt"));
            var total = Math.Max(1, reports.Count);

            PendingLabel.Text = $"Pendientes: {pending}";
            InProgressLabel.Text = $"En atención: {progress}";
            ClosedLabel.Text = $"Cerrados: {closed}";
            PendingBar.Progress = (double)pending / total;
            InProgressBar.Progress = (double)progress / total;
            ClosedBar.Progress = (double)closed / total;

            SummaryLabel.Text = $"La aplicación registra {activities} actividades comunitarias, {reports.Count} reportes urbanos, {cases} alertas o casos sociales y {requests + works} trámites o propuestas ciudadanas.";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Estadísticas", $"No se pudo actualizar el panel: {ex.Message}", "Aceptar");
        }
        finally
        {
            LoadingIndicator.IsVisible = LoadingIndicator.IsRunning = false;
        }
    }

    private static int Count<T>(ApiResult<List<T>> result) => result.IsSuccess ? result.Data?.Count ?? 0 : 0;

    private static bool Contains(string? value, params string[] terms)
    {
        var text = value?.ToLowerInvariant() ?? string.Empty;
        return terms.Any(text.Contains);
    }

    private async void OnRefreshClicked(object sender, EventArgs e) => await LoadAsync();
}
