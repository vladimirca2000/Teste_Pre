@echo off
REM Script para iniciar projeto Angular meu-teste-front na porta 4200

echo.
echo ============================================
echo  Iniciando Projeto Angular - meu-teste-front
echo ============================================
echo.

cd /d "%~dp0"

echo Instalando dependências (se necessário)...
if not exist "node_modules" (
    echo npm install
    call npm install
)

echo.
echo ============================================
echo  Iniciando servidor na porta 4200...
echo ============================================
echo.
echo Acesse: http://localhost:4200/
echo.

npm start

pause
