@echo off
REM ============================================================================
REM SCRIPT COMPLETO PARA SETUP E EXECUÇÃO - BACKEND E FRONTEND
REM ============================================================================
REM Autor: GitHub Copilot
REM Data: 2026-01-08
REM Descrição: Gerencia a execução do Backend e Frontend em sequência
REM ============================================================================

setlocal enabledelayedexpansion

REM ============================================================================
REM CONFIGURAÇÃO INICIAL
REM ============================================================================
title MeuTeste - Backend + Frontend Manager
color 0A

set BACKEND_PATH=C:\Users\hibit\source\repos\teste\back\MeuTeste
set FRONTEND_PATH=C:\Users\hibit\source\repos\teste\back\MeuTeste\meu-teste-front
set BACKEND_PORT=5000
set FRONTEND_PORT=4200

REM ============================================================================
REM FUNÇÃO: EXIBIR MENU PRINCIPAL
REM ============================================================================
:MENU_PRINCIPAL
cls
echo.
echo ============================================================================
echo                   MEUTESTE - BACKEND E FRONTEND MANAGER
echo ============================================================================
echo.
echo [1] Executar Backend + Frontend (Sequencial)
echo [2] Apenas Backend
echo [3] Apenas Frontend
echo [4] Testar Backend com CURL
echo [5] Lipar node_modules e reinstalar
echo [6] Sair
echo.
echo ============================================================================
echo.
set /p choice="Escolha uma opcao (1-6): "

if "%choice%"=="1" goto EXEC_BACKEND_AND_FRONTEND
if "%choice%"=="2" goto EXEC_BACKEND_ONLY
if "%choice%"=="3" goto EXEC_FRONTEND_ONLY
if "%choice%"=="4" goto TEST_BACKEND
if "%choice%"=="5" goto CLEAN_INSTALL
if "%choice%"=="6" goto EXIT_SCRIPT
goto MENU_PRINCIPAL

REM ============================================================================
REM OPÇÃO 1: EXECUTAR BACKEND + FRONTEND (SEQUENCIAL)
REM ============================================================================
:EXEC_BACKEND_AND_FRONTEND
cls
echo.
echo ============================================================================
echo                    EXECUTANDO BACKEND + FRONTEND
echo ============================================================================
echo.

REM Verificar caminhos
if not exist "%BACKEND_PATH%" (
    echo ? ERRO: Caminho do Backend nao encontrado:
    echo    %BACKEND_PATH%
    echo.
    pause
    goto MENU_PRINCIPAL
)

if not exist "%FRONTEND_PATH%" (
    echo ? ERRO: Caminho do Frontend nao encontrado:
    echo    %FRONTEND_PATH%
    echo.
    pause
    goto MENU_PRINCIPAL
)

REM ============================================================================
REM FASE 1: PREPARAR E COMPILAR BACKEND
REM ============================================================================
echo [FASE 1/4] Preparando Backend...
echo    Caminho: %BACKEND_PATH%
echo.

cd /d "%BACKEND_PATH%"

echo [FASE 2/4] Compilando Backend...
echo    Executando: dotnet build
echo.

dotnet build

if %errorlevel% neq 0 (
    echo.
    echo ? ERRO: Compilacao do Backend falhou!
    echo.
    pause
    goto MENU_PRINCIPAL
)

echo.
echo ? Backend compilado com sucesso!
echo.

REM ============================================================================
REM FASE 2: EXECUTAR BACKEND
REM ============================================================================
echo [FASE 3/4] Iniciando Backend...
echo.
echo    URL Backend: http://localhost:%BACKEND_PORT%
echo    Swagger: http://localhost:%BACKEND_PORT%/swagger
echo.
echo ? Backend iniciando...
echo.
echo ============================================================================
echo                        BACKEND RODANDO
echo ============================================================================
echo.
echo AGUARDE: O Backend deve mostrar "Application started" abaixo
echo.

dotnet run --project MeuTeste.Presentation

REM Se chegou aqui, backend foi fechado
goto MENU_PRINCIPAL

REM ============================================================================
REM OPÇÃO 2: APENAS BACKEND
REM ============================================================================
:EXEC_BACKEND_ONLY
cls
echo.
echo ============================================================================
echo                         EXECUTANDO APENAS BACKEND
echo ============================================================================
echo.

if not exist "%BACKEND_PATH%" (
    echo ? ERRO: Caminho do Backend nao encontrado
    echo    %BACKEND_PATH%
    pause
    goto MENU_PRINCIPAL
)

cd /d "%BACKEND_PATH%"

echo Compilando...
dotnet build

if %errorlevel% neq 0 (
    echo.
    echo ? Compilacao falhou!
    pause
    goto MENU_PRINCIPAL
)

echo.
echo ? Compilacao concluida!
echo.
echo Iniciando Backend em http://localhost:%BACKEND_PORT%
echo.

dotnet run --project MeuTeste.Presentation

goto MENU_PRINCIPAL

REM ============================================================================
REM OPÇÃO 3: APENAS FRONTEND
REM ============================================================================
:EXEC_FRONTEND_ONLY
cls
echo.
echo ============================================================================
echo                         EXECUTANDO APENAS FRONTEND
echo ============================================================================
echo.

if not exist "%FRONTEND_PATH%" (
    echo ? ERRO: Caminho do Frontend nao encontrado
    echo    %FRONTEND_PATH%
    pause
    goto MENU_PRINCIPAL
)

cd /d "%FRONTEND_PATH%"

echo Instalando dependencias...
echo (Este processo pode levar alguns minutos na primeira vez)
echo.
npm install

if %errorlevel% neq 0 (
    echo.
    echo ? npm install falhou!
    pause
    goto MENU_PRINCIPAL
)

echo.
echo ? Dependencias instaladas!
echo.
echo Iniciando Frontend em http://localhost:%FRONTEND_PORT%
echo.

npx ng serve

goto MENU_PRINCIPAL

REM ============================================================================
REM OPÇÃO 4: TESTAR BACKEND COM CURL
REM ============================================================================
:TEST_BACKEND
cls
echo.
echo ============================================================================
echo                      TESTANDO BACKEND COM CURL
echo ============================================================================
echo.
echo ??  IMPORTANTE: Backend deve estar rodando em http://localhost:%BACKEND_PORT%
echo.
echo [1] Testar Login (POST /api/auth/login)
echo [2] Testar Categorias (GET /api/categories)
echo [3] Testar Usuarios Inativos (GET /api/users/inactive)
echo [4] Voltar ao Menu
echo.
set /p test_choice="Escolha uma opcao (1-4): "

if "%test_choice%"=="1" goto TEST_LOGIN
if "%test_choice%"=="2" goto TEST_CATEGORIES
if "%test_choice%"=="3" goto TEST_USERS
if "%test_choice%"=="4" goto MENU_PRINCIPAL
goto TEST_BACKEND

:TEST_LOGIN
cls
echo.
echo Testando Login...
echo.
curl -X POST http://localhost:%BACKEND_PORT%/api/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"admin\",\"password\":\"admin123\"}"
echo.
echo.
pause
goto TEST_BACKEND

:TEST_CATEGORIES
cls
echo.
echo ??  Este teste requer um token JWT válido
echo.
set /p token="Cole o token JWT (ou pressione ENTER para usar um token de teste): "
if "%token%"=="" (
    echo.
    echo Usando teste sem autenticação...
    curl http://localhost:%BACKEND_PORT%/api/categories
) else (
    echo.
    echo Testando com token...
    curl -H "Authorization: Bearer %token%" http://localhost:%BACKEND_PORT%/api/categories
)
echo.
echo.
pause
goto TEST_BACKEND

:TEST_USERS
cls
echo.
echo ??  Este teste requer um token JWT de ADMIN
echo.
set /p token="Cole o token JWT do Admin: "
if "%token%"=="" (
    echo.
    echo ? Token nao fornecido!
) else (
    echo.
    echo Testando usuarios inativos...
    curl -H "Authorization: Bearer %token%" http://localhost:%BACKEND_PORT%/api/users/inactive
)
echo.
echo.
pause
goto TEST_BACKEND

REM ============================================================================
REM OPÇÃO 5: LIMPAR node_modules E REINSTALAR
REM ============================================================================
:CLEAN_INSTALL
cls
echo.
echo ============================================================================
echo                  LIMPANDO E REINSTALANDO FRONTEND
echo ============================================================================
echo.

if not exist "%FRONTEND_PATH%" (
    echo ? ERRO: Caminho do Frontend nao encontrado
    pause
    goto MENU_PRINCIPAL
)

cd /d "%FRONTEND_PATH%"

echo Removendo node_modules...
if exist "node_modules" (
    rmdir /s /q node_modules
    echo ? node_modules removido
) else (
    echo ??  node_modules nao existe
)

echo.
echo Removendo package-lock.json...
if exist "package-lock.json" (
    del package-lock.json
    echo ? package-lock.json removido
) else (
    echo ??  package-lock.json nao existe
)

echo.
echo Limpando cache Angular...
if exist ".angular" (
    rmdir /s /q .angular
    echo ? .angular removido
) else (
    echo ??  .angular nao existe
)

echo.
echo Reinstalando dependencias...
npm install

if %errorlevel% neq 0 (
    echo.
    echo ? npm install falhou!
    pause
    goto MENU_PRINCIPAL
)

echo.
echo ? Frontend reinstalado com sucesso!
echo.
pause
goto MENU_PRINCIPAL

REM ============================================================================
REM SAIR
REM ============================================================================
:EXIT_SCRIPT
cls
echo.
echo ============================================================================
echo                              ATE LOGO!
echo ============================================================================
echo.
echo Obrigado por usar o MeuTeste Manager!
echo.
pause
exit /b 0
