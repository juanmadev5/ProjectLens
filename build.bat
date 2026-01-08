@echo off
set PROJECT_NAME=ProjectLens
set OUTPUT_DIR=bin\Release\net10.0\

echo [1/3] Compilando para Windows x64 (64-bit)...
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:DebugType=None /p:DebugSymbols=false

echo [2/3] Compilando para Windows x86 (32-bit)...
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true /p:DebugType=None /p:DebugSymbols=false 

echo [3/3] Proceso finalizado.
echo Los ejecutables estan en la carpeta: %OUTPUT_DIR% para x86 y x64 dentro de publish.
pause