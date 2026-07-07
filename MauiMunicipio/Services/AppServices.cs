namespace MauiMunicipio.Services;

public static class AppServices
{
    public static ApiService Api { get; } = new();
    public static SessionService Session { get; } = new();
}
