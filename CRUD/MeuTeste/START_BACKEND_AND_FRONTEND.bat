@echo off
REM ============================================================================
REM SCRIPT PARA EXECUTAR BACKEND E FRONTEND EM SEQU�NCIA
REM ============================================================================
REM Este script:
REM 1. Navega para a pasta do Backend
REM 2. Compila o Backend (dotnet build)
REM 3. Executa o Backend (dotnet run)
REM 4. Aguarda confirma��o do usu�rio
REM 5. Se confirmado (Y), abre um novo terminal com o Frontend
REM ============================================================================

setlocal enabledelayedexpansion

REM Cores para output (opcional, usando ANSI escape codes)
cls
echo.
echo ============================================================================
echo        EXECUTAR BACKEND E FRONTEND EM SEQUENCIA
echo ============================================================================
echo.

REM Definir caminhos
set BACKEND_PATH=C:\Users\hibit\source\repos\teste\CRUD\MeuTeste
set FRONTEND_PATH=C:\Users\hibit\source\repos\teste\CRUD\MeuTeste\meu-teste-front

REM Verificar se os caminhos existem
if not exist "%BACKEND_PATH%" (
    echo ? ERRO: Caminho do Backend nao encontrado:
    echo    %BACKEND_PATH%
    echo.
    pause
    exit /b 1
)

if not exist "%FRONTEND_PATH%" (
    echo ? ERRO: Caminho do Frontend nao encontrado:
    echo    %FRONTEND_PATH%
    echo.
    pause
    exit /b 1
)

REM ============================================================================
REM FASE 1: COMPILAR E EXECUTAR BACKEND
REM ============================================================================
echo.
echo [1/3] Navegando para Backend...
echo    %BACKEND_PATH%
echo.
cd /d "%BACKEND_PATH%"

echo [2/3] Compilando Backend...
echo    Executando: dotnet build
echo.
dotnet build

if %errorlevel% neq 0 (
    echo.
    echo ? ERRO: Compilacao do Backend falhou!
    echo.
    pause
    exit /b 1
)

echo.
echo ? Backend compilado com sucesso!
echo.
echo [3/3] Iniciando Backend...
echo    Url: http://localhost:5000
echo    Swagger: http://localhost:5000/swagger
echo.
echo ? Aguardando Backend iniciar...
echo.

REM Executar backend em modo backgroundREM Vamos executar o backend em uma forma que permita continua��o
dotnet run --project MeuTeste.Presentation

REM Se chegou aqui, backend foi fechado
if %errorlevel% neq 0 (
    echo.
    echo ? ERRO: Backend nao conseguiu iniciar!
    echo.
    pause
    exit /b 1
)

REM ============================================================================
REM FASE 2: AGUARDAR CONFIRMA��O DO USU�RIO
REM ============================================================================
echo.
echo.
echo ============================================================================
echo        ? BACKEND INICIADO COM SUCESSO!
echo ============================================================================
echo.
echo Backend rodando em: http://localhost:5000
echo Swagger em: http://localhost:5000/swagger
echo.
echo.
echo PROXIMOS PASSOS:
echo ? Teste o Backend em outro terminal (CURL tests)
echo ? Verifique se tudo est� funcionando
echo ? Quando pronto, confirme para iniciar o Frontend
echo.
set /p continue="Deseja continuar com o Frontend? (S/N): "

if /i NOT "%continue%"=="S" (
    echo.
    echo ? Execucao do Frontend cancelada pelo usuario.
    echo.
    pause
    exit /b 0
)

REM ============================================================================
REM FASE 3: EXECUTAR FRONTEND EM UM NOVO TERMINAL
REM ============================================================================
echo.
echo.
echo ============================================================================
echo        INICIANDO FRONTEND
echo ============================================================================
echo.
echo Abrindo novo terminal para Frontend...
echo Caminho: %FRONTEND_PATH%
echo.

REM Abre um novo terminal (cmd.exe) e executa os comandos do frontend
start cmd.exe /k "cd /d "%FRONTEND_PATH%" && echo. && echo ============== FRONTEND ============== && echo. && npm install && echo. && echo ? npm install concluido! && echo. && ng serve"

echo.
echo ? Terminal do Frontend aberto!
echo.
echo Frontend sera inicializado no novo terminal...
echo Acesse em: http://localhost:4200
echo.
pause
