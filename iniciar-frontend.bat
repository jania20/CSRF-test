@echo off
echo ========================================
echo   Iniciando Frontend (Servidor HTTP)
echo ========================================
echo.

cd /d "%~dp0"

echo Verificando Python...
python --version >nul 2>&1
if errorlevel 1 (
    echo Python no encontrado. Intentando con 'py'...
    py --version >nul 2>&1
    if errorlevel 1 (
        echo.
        echo ERROR: Python no esta instalado
        echo Por favor instala Python desde: https://www.python.org/downloads/
        echo O usa Node.js: npx http-server -p 8000
        pause
        exit /b 1
    )
    echo Usando: py -m http.server 8000
    echo.
    echo El frontend estara en: http://localhost:8000
    echo.
    echo Presiona Ctrl+C para detener el servidor
    echo.
    py -m http.server 8000
) else (
    echo Usando: python -m http.server 8000
    echo.
    echo El frontend estara en: http://localhost:8000
    echo.
    echo Presiona Ctrl+C para detener el servidor
    echo.
    python -m http.server 8000
)

pause

