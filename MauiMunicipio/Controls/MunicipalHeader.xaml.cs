namespace MauiMunicipio.Controls;

public partial class MunicipalHeader : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(MunicipalHeader), "Municipio de Quito");

    public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(
        nameof(Subtitle), typeof(string), typeof(MunicipalHeader), "Servicios Ciudadanos");

    public static readonly BindableProperty ShowBackProperty = BindableProperty.Create(
        nameof(ShowBack), typeof(bool), typeof(MunicipalHeader), false);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public bool ShowBack
    {
        get => (bool)GetValue(ShowBackProperty);
        set => SetValue(ShowBackProperty, value);
    }

    public event EventHandler? BackClicked;
    public event EventHandler? MenuClicked;

    public MunicipalHeader()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (BackClicked is not null)
            BackClicked.Invoke(this, EventArgs.Empty);
        else
            await Shell.Current.GoToAsync("..");
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (MenuClicked is not null)
            MenuClicked.Invoke(this, EventArgs.Empty);
        else
            await Shell.Current.GoToAsync(nameof(MauiMunicipio.Pages.ProfilePage));
    }
}
