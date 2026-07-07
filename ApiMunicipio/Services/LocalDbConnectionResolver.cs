using System.Diagnostics;

namespace ApiMunicipio.Services;

/// <summary>
/// Resuelve la canalización actual de una instancia LocalDB. La canalización cambia
/// cada vez que LocalDB se reinicia, por lo que no debe guardarse de forma fija en
/// appsettings.json.
/// </summary>
public static class LocalDbConnectionResolver
{
    public static string Resolve(IConfiguration configuration)
    {
        var configured = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No existe ConnectionStrings:DefaultConnection.");

        if (!OperatingSystem.IsWindows())
            return configured;

        var instanceName = configuration["Database:LocalDbInstance"] ?? "AppMunicipioLocal";
        var databaseName = configuration["Database:Name"] ?? "AppMunicipio_Database";

        try
        {
            var executable = FindSqlLocalDbExecutable();
            var info = Run(executable, "info", instanceName);

            if (info.ExitCode != 0)
            {
                var create = Run(executable, "create", instanceName, "-s");
                if (create.ExitCode != 0)
                    return configured;
            }
            else
            {
                // Si ya está iniciada, start devuelve un mensaje informativo; no es un problema.
                Run(executable, "start", instanceName);
            }

            info = Run(executable, "info", instanceName);
            var output = $"{info.StandardOutput}\n{info.StandardError}";
            var pipe = output
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .FirstOrDefault(line => line.Contains("np:\\\\.\\pipe\\", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(pipe))
                return configured;

            var pipeStart = pipe.IndexOf("np:\\\\.\\pipe\\", StringComparison.OrdinalIgnoreCase);
            pipe = pipe[pipeStart..].Trim();

            return $"Server={pipe};Database={databaseName};Integrated Security=True;" +
                   "MultipleActiveResultSets=True;TrustServerCertificate=True";
        }
        catch
        {
            // Permite usar SQL Server normal o una conexión personalizada si LocalDB no está disponible.
            return configured;
        }
    }

    private static string FindSqlLocalDbExecutable()
    {
        const string executable = "SqlLocalDB.exe";
        var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        var sqlRoot = Path.Combine(programFiles, "Microsoft SQL Server");

        if (Directory.Exists(sqlRoot))
        {
            try
            {
                var located = Directory
                    .EnumerateFiles(sqlRoot, executable, SearchOption.AllDirectories)
                    .OrderByDescending(path => path, StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(located))
                    return located;
            }
            catch
            {
                // Se intentará resolver desde PATH.
            }
        }

        return executable;
    }

    private static ProcessResult Run(string executable, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("No se pudo ejecutar SqlLocalDB.exe.");

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit(15000);

        return new ProcessResult(process.ExitCode, stdout, stderr);
    }

    private sealed record ProcessResult(int ExitCode, string StandardOutput, string StandardError);
}
