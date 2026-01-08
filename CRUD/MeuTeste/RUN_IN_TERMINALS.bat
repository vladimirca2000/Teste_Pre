@echo off
REM ============================================================================
REM SCRIPT AVANÇADO: BACKEND EM UM TERMINAL + FRONTEND EM OUTRO
REM ============================================================================
REM Este script abre dois terminais simultaneamente:
REM - Terminal 1: Backend
REM - Terminal 2: Frontend (após confirmação)
REM ============================================================================

setlocal enabledelayedexpansion
title MeuTeste - Terminal Manager

REM Configuração
set BACKEND_PATH=C:\Users\hibit\source\repos\teste\back\MeuTeste
set FRONTEND_PATH=%BACKEND_PATH%\meu-teste-front

REM Cores
color 0A

REM ============================================================================
REM VERIFICAR CAMINHOS
REM ============================================================================
if not exist "%BACKEND_PATH%" (
    cls
    echo ? Backend nao encontrado: %BACKEND_PATH%
    pause
    exit /b 1
)

if not exist "%FRONTEND_PATH%" (
    cls
    echo ? Frontend nao encontrado: %FRONTEND_PATH%
    pause
    exit /b 1
)

REM ============================================================================
REM MENU INICIAL
REM ============================================================================
:MENU
cls
echo.
echo ============================================================================
echo                    MEUTESTE - TERMINAL MANAGER
echo ============================================================================
echo.
echo [1] Abrir Backend em novo Terminal (recomendado)
echo [2] Abrir Frontend em novo Terminal
echo [3] Abrir Backend + Frontend (dois terminais)
echo [4] Sair
echo.
set /p choice="Escolha uma opcao: "

if "%choice%"=="1" goto OPEN_BACKEND_TERMINAL
if "%choice%"=="2" goto OPEN_FRONTEND_TERMINAL
if "%choice%"=="3" goto OPEN_BOTH_TERMINALS
if "%choice%"=="4" exit /b 0
goto MENU

REM ============================================================================
REM OPÇÃO 1: ABRIR BACKEND EM NOVO TERMINAL
REM ============================================================================
:OPEN_BACKEND_TERMINAL
cls
echo.
echo ============================================================================
echo                    ABRINDO BACKEND EM NOVO TERMINAL
echo ============================================================================
echo.
echo Abrindo: %BACKEND_PATH%
echo.

start "MeuTeste - BACKEND" cmd.exe /k "cd /d "%BACKEND_PATH%" && color 0A && cls && echo. && echo ============== BACKEND ============== && echo. && echo Compilando... && echo. && dotnet build && cls && echo. && echo ? Backend compilado! && echo. && echo Iniciando em http://localhost:5000 && echo Swagger em http://localhost:5000/swagger && echo. && dotnet run --project MeuTeste.Presentation"

echo ? Terminal do Backend aberto!
echo.
echo Verifique o novo terminal para ver o Backend rodando.
echo.
pause
goto MENU

REM ============================================================================
REM OPÇÃO 2: ABRIR FRONTEND EM NOVO TERMINAL
REM ============================================================================
:OPEN_FRONTEND_TERMINAL
cls
echo.
echo ============================================================================
echo                    ABRINDO FRONTEND EM NOVO TERMINAL
echo ============================================================================
echo.
echo Abrindo: %FRONTEND_PATH%
echo.

start "MeuTeste - FRONTEND" cmd.exe /k "cd /d "%FRONTEND_PATH%" && color 0B && cls && echo. && echo ============== FRONTEND ============== && echo. && echo Instalando dependencias... && echo (Este processo pode levar alguns minutos) && echo. && npm install && cls && echo. && echo ? Dependencias instaladas! && echo. && echo Iniciando em http://localhost:4200 && echo. && npx ng serve"

echo ? Terminal do Frontend aberto!
echo.
echo Verifique o novo terminal para ver o Frontend rodando.
echo.
pause
goto MENU

REM ============================================================================
REM OPÇÃO 3: ABRIR BACKEND + FRONTEND (DOIS TERMINAIS)
REM ============================================================================
:OPEN_BOTH_TERMINALS
cls
echo.
echo ============================================================================
echo                    ABRINDO BACKEND + FRONTEND
echo ============================================================================
echo.
echo Abrindo dois terminais...
echo.

REM Abrir Backend em novo terminal
echo Abrindo Backend em novo terminal...
start "MeuTeste - BACKEND" cmd.exe /k "cd /d "%BACKEND_PATH%" && color 0A && cls && echo. && echo ============== BACKEND ============== && echo. && echo Compilando... && echo. && dotnet build && cls && echo. && echo ? Backend compilado! && echo. && echo Iniciando em http://localhost:5000 && echo Swagger em http://localhost:5000/swagger && echo. && echo ? Aguarde o Backend iniciar completamente... && echo. && dotnet run --project MeuTeste.Presentation"

REM Aguardar um pouco para backend ter tempo de iniciar
timeout /t 3 /nobreak

REM Perguntar se quer abrir o frontend
echo.
set /p continue="Deseja abrir o Frontend? (S/N): "

if /i "%continue%"=="S" (
    echo.
    echo Abrindo Frontend em novo terminal...
    start "MeuTeste - FRONTEND" cmd.exe /k "cd /d "%FRONTEND_PATH%" && color 0B && cls && echo. && echo ============== FRONTEND ============== && echo. && echo Instalando dependencias... && echo (Este processo pode levar alguns minutos) && echo. && npm install && cls && echo. && echo ? Dependencias instaladas! && echo. && echo Iniciando em http://localhost:4200 && echo. && npx ng serve"
    
    echo.
    echo ? Ambos os terminais abertos!
    echo.
    echo Backend:  http://localhost:5000
    echo Frontend: http://localhost:4200
    echo.
) else (
    echo.
    echo Frontend nao sera aberto.
    echo.
)

pause
goto MENU
