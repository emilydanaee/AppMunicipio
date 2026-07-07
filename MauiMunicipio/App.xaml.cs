namespace MauiMunicipio;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
#if WINDOWS
        window.Width = 460;
        window.Height = 850;
        window.MinimumWidth = 390;
        window.MinimumHeight = 700;
#endif
        return window;
    }
}
