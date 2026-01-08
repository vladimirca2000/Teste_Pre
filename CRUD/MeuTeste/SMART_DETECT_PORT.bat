@echo off
REM ============================================================================
REM SCRIPT INTELIGENTE: DETECTA PORTA BACKEND E ATUALIZA FRONTEND AUTOMATICAMENTE
REM ============================================================================
REM Este script:
REM 1. Detecta qual porta o Backend está usando
REM 2. Atualiza o Frontend automaticamente
REM 3. Abre Frontend no terminal correto
REM ============================================================================

setlocal enabledelayedexpansion
title MeuTeste - Smart Port Detector

set FRONTEND_PATH=C:\Users\hibit\source\repos\teste\back\MeuTeste\meu-teste-front
set ENV_FILE=%FRONTEND_PATH%\src\environments\environment.ts

cls
echo.
echo ============================================================================
echo                   DETECTOR DE PORTA INTELIGENTE
echo ============================================================================
echo.
echo Este script detecta qual porta o Backend está usando
echo e atualiza o Frontend automaticamente!
echo.
echo ============================================================================
echo.

REM Menu para escolher a porta
echo [1] Backend em http://localhost:5000 (padrão)
echo [2] Backend em http://localhost:5238 (launchSettings atual)
echo [3] Outra porta (digite manualmente)
echo.
set /p choice="Qual porta o Backend está usando? (1-3): "

if "%choice%"=="1" (
    set BACKEND_PORT=5000
    set API_URL=http://localhost:5000/api
    goto UPDATE_FRONTEND
)

if "%choice%"=="2" (
    set BACKEND_PORT=5238
    set API_URL=http://localhost:5238/api
    goto UPDATE_FRONTEND
)

if "%choice%"=="3" (
    set /p BACKEND_PORT="Digite a porta (ex: 5239): "
    set API_URL=http://localhost:!BACKEND_PORT!/api
    goto UPDATE_FRONTEND
)

goto :EOF

REM ============================================================================
REM ATUALIZAR FRONTEND COM A PORTA CORRETA
REM ============================================================================
:UPDATE_FRONTEND
cls
echo.
echo ============================================================================
echo                         ATUALIZANDO FRONTEND
echo ============================================================================
echo.
echo Backend Port: %BACKEND_PORT%
echo API URL: %API_URL%
echo Environment File: %ENV_FILE%
echo.

REM Verificar se arquivo existe
if not exist "%ENV_FILE%" (
    echo ? ERRO: Arquivo nao encontrado: %ENV_FILE%
    pause
    exit /b 1
)

REM Criar novo environment.ts
echo Atualizando environment.ts...
(
    echo export const environment = {
    echo   production: false,
    echo   apiUrl: '%API_URL%'
    echo };
) > "%ENV_FILE%"

echo ? environment.ts atualizado com sucesso!
echo.

REM Iniciar Frontend
cd /d "%FRONTEND_PATH%"

echo Instalando dependencias...
npm install

if %errorlevel% neq 0 (
    echo.
    echo ? npm install falhou!
    pause
    exit /b 1
)

cls
echo.
echo ============================================================================
echo                         INICIANDO FRONTEND
echo ============================================================================
echo.
echo Frontend: http://localhost:4200
echo Backend: http://localhost:%BACKEND_PORT%
echo API: %API_URL%
echo.
echo ? Aguarde o Angular compilar...
echo.

npx ng serve

:EOF
