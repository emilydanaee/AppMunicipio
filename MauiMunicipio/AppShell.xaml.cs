using MauiMunicipio.Pages;

namespace MauiMunicipio;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(RequestsPage), typeof(RequestsPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(ActivityDetailPage), typeof(ActivityDetailPage));
        Routing.RegisterRoute(nameof(ApiSettingsPage), typeof(ApiSettingsPage));
        Routing.RegisterRoute(nameof(MyRegistrationsPage), typeof(MyRegistrationsPage));
        Routing.RegisterRoute(nameof(MyReportsPage), typeof(MyReportsPage));
        Routing.RegisterRoute(nameof(NewPermitPage), typeof(NewPermitPage));
        Routing.RegisterRoute(nameof(NewReservationPage), typeof(NewReservationPage));
        Routing.RegisterRoute(nameof(ManagementPage), typeof(ManagementPage));
        Routing.RegisterRoute(nameof(UniversalFormPage), typeof(UniversalFormPage));
        Routing.RegisterRoute(nameof(AttendeesPage), typeof(AttendeesPage));
        Routing.RegisterRoute(nameof(DashboardStatsPage), typeof(DashboardStatsPage));
    }
}
