@echo off
setlocal
cd /d "%~dp0"
echo Cerrando archivos temporales del proyecto...
if exist ".vs" rmdir /s /q ".vs"
for %%P in (ApiMunicipio MauiMunicipio WebMunicipio) do (
    if exist "%%P\bin" rmdir /s /q "%%P\bin"
    if exist "%%P\obj" rmdir /s /q "%%P\obj"
)
echo Listo. Abriendo AppMunicipio.sln...
start "" "AppMunicipio.sln"
endlocal
