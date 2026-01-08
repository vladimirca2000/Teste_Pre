@echo off
REM ============================================================================
REM SCRIPT SIMPLES: EXECUTAR BACKEND + AGUARDAR CONFIRMA��O + EXECUTAR FRONTEND
REM ============================================================================
REM Este � o script mais simples e direto
REM 1. Executa Backend
REM 2. Aguarda confirma��o (Y/N)
REM 3. Se sim, abre um novo terminal com Frontend
REM ============================================================================

setlocal enabledelayedexpansion
title MeuTeste - Backend First

REM Cores
color 0A

REM Configura��o
set BACKEND_PATH=C:\Users\hibit\source\repos\teste\CRUD\MeuTeste
set FRONTEND_PATH=%BACKEND_PATH%\meu-teste-front

REM ============================================================================
REM VERIFICAR SE OS CAMINHOS EXISTEM
REM ============================================================================
if not exist "%BACKEND_PATH%" (
    echo ? Backend nao encontrado em: %BACKEND_PATH%
    pause
    exit /b 1
)

if not exist "%FRONTEND_PATH%" (
    echo ? Frontend nao encontrado em: %FRONTEND_PATH%
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
echo                    MEUTESTE - BACKEND E FRONTEND
echo ============================================================================
echo.
echo [1] Executar Backend e Frontend
echo [2] Apenas Backend
echo [3] Apenas Frontend
echo [4] Sair
echo.
set /p menu_choice="Escolha uma opcao: "

if "%menu_choice%"=="1" goto BACKEND_FIRST
if "%menu_choice%"=="2" goto BACKEND_ONLY
if "%menu_choice%"=="3" goto FRONTEND_ONLY
if "%menu_choice%"=="4" exit /b 0
goto MENU

REM ============================================================================
REM OP��O 1: BACKEND PRIMEIRO, DEPOIS FRONTEND
REM ============================================================================
:BACKEND_FIRST
cls
echo.
echo ============================================================================
echo                         COMPILANDO BACKEND
echo ============================================================================
echo.
cd /d "%BACKEND_PATH%"
dotnet build
if %errorlevel% neq 0 (
    echo.
    echo ? ERRO na compilacao!
    pause
    goto MENU
)

cls
echo.
echo ============================================================================
echo                         INICIANDO BACKEND
echo ============================================================================
echo.
echo Backend: http://localhost:5000
echo Swagger: http://localhost:5000/swagger
echo.

REM Executar backend
dotnet run --project MeuTeste.Presentation

REM Se o backend foi encerrado
goto MENU

REM ============================================================================
REM OP��O 2: APENAS BACKEND
REM ============================================================================
:BACKEND_ONLY
cls
echo.
echo ============================================================================
echo                         BACKEND (MODO ISOLADO)
echo ============================================================================
echo.
cd /d "%BACKEND_PATH%"
dotnet build
if %errorlevel% neq 0 (
    echo.
    echo ? ERRO na compilacao!
    pause
    goto MENU
)

cls
echo.
echo ============================================================================
echo                         INICIANDO BACKEND
echo ============================================================================
echo.
echo Backend: http://localhost:5000
echo Swagger: http://localhost:5000/swagger
echo.

dotnet run --project MeuTeste.Presentation

goto MENU

REM ============================================================================
REM OP��O 3: APENAS FRONTEND
REM ============================================================================
:FRONTEND_ONLY
cls
echo.
echo ============================================================================
echo                         FRONTEND (MODO ISOLADO)
echo ============================================================================
echo.
cd /d "%FRONTEND_PATH%"

echo Instalando dependencias...
npm install

if %errorlevel% neq 0 (
    echo.
    echo ? ERRO no npm install!
    pause
    goto MENU
)

cls
echo.
echo ============================================================================
echo                         INICIANDO FRONTEND
echo ============================================================================
echo.
echo Frontend: http://localhost:4200
echo.

npx ng serve

goto MENU
