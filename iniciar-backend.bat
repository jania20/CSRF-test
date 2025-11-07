@echo off
echo ========================================
echo   Iniciando Backend (CSRF API)
echo ========================================
echo.

cd /d "%~dp0Backend\CSRF API"

echo Verificando .NET...
dotnet --version
if errorlevel 1 (
    echo.
    echo ERROR: .NET no esta instalado o no esta en el PATH
    echo Por favor instala .NET SDK desde: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo.
echo Ejecutando backend...
echo El servidor estara en: http://localhost:5029
echo.
echo Presiona Ctrl+C para detener el servidor
echo.

dotnet run

pause

