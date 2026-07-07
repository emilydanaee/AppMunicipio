@echo off
setlocal
cd /d "%~dp0"
echo Limpiando compilacion anterior de MAUI...
if exist "MauiMunicipio\bin" rmdir /s /q "MauiMunicipio\bin"
if exist "MauiMunicipio\obj" rmdir /s /q "MauiMunicipio\obj"
echo Restaurando paquetes para Windows ARM64...
dotnet restore "MauiMunicipio\MauiMunicipio.csproj" -p:RuntimeIdentifier=win-arm64 -p:UseMonoRuntime=false -p:WindowsPackageType=None
if errorlevel 1 goto error
echo Compilando MAUI...
dotnet build "MauiMunicipio\MauiMunicipio.csproj" -f net9.0-windows10.0.19041.0 -p:RuntimeIdentifier=win-arm64 -p:UseMonoRuntime=false -p:WindowsPackageType=None --no-restore
if errorlevel 1 goto error
echo.
echo COMPILACION CORRECTA. Regresa a Visual Studio y ejecuta Windows Machine.
pause
exit /b 0
:error
echo.
echo La compilacion fallo. Revisa las lineas rojas anteriores.
pause
exit /b 1
