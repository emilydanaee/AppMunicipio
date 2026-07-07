using System.Text.Json;
using MauiMunicipio.Models;

namespace MauiMunicipio.Services;

public sealed class SessionService
{
    private const string SessionKey = "municipio_user_session";
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    private UserSession? _current;
    private bool _loaded;

    public event EventHandler? SessionChanged;

    public UserSession? Current => _current;
    public bool IsLoggedIn => _current is not null;

    public async Task<UserSession?> LoadAsync()
    {
        if (_loaded)
            return _current;

        _loaded = true;
        try
        {
            var json = await SecureStorage.Default.GetAsync(SessionKey);
            if (!string.IsNullOrWhiteSpace(json))
                _current = JsonSerializer.Deserialize<UserSession>(json, _jsonOptions);
        }
        catch
        {
            var json = Preferences.Default.Get(SessionKey, string.Empty);
            if (!string.IsNullOrWhiteSpace(json))
                _current = JsonSerializer.Deserialize<UserSession>(json, _jsonOptions);
        }

        return _current;
    }

    public async Task SaveAsync(UserSession session)
    {
        _current = session;
        _loaded = true;
        var json = JsonSerializer.Serialize(session, _jsonOptions);

        try
        {
            await SecureStorage.Default.SetAsync(SessionKey, json);
        }
        catch
        {
            Preferences.Default.Set(SessionKey, json);
        }

        SessionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        _current = null;
        _loaded = true;
        try
        {
            SecureStorage.Default.Remove(SessionKey);
        }
        catch
        {
            // Se elimina también del respaldo de Preferences.
        }
        Preferences.Default.Remove(SessionKey);
        SessionChanged?.Invoke(this, EventArgs.Empty);
    }
}
