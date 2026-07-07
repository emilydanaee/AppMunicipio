@echo off
setlocal
cd /d "%~dp0"
echo Iniciando ApiMunicipio en http://localhost:5279 ...
dotnet run --project "ApiMunicipio\ApiMunicipio.csproj" --launch-profile http
if errorlevel 1 (
    echo.
    echo La API no pudo iniciar. Revisa el mensaje anterior y confirma que .NET 8 este instalado.
    pause
)
endlocal
