@echo off
echo ========================================
echo   Student Information System - API
echo ========================================
echo.

cd /d "%~dp0SIS.Api"

echo Restoring packages...
dotnet restore

echo.
echo Starting API on http://localhost:5000 ...
echo Press CTRL+C to stop.
echo.
dotnet run

pause
