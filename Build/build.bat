@echo off
SETLOCAL EnableDelayedExpansion

echo Start building...

if not exist %cd%\output\ (
  echo Creating output directory
  mkdir %cd%\output\
)

if exist D:\output\ (
  rmdir /S /Q D:\output\
)

mkdir D:\output\

echo Current directory: '%cd%'

set /A exitCode=0

"C:\Program Files\Unity\Hub\Editor\2019.4.18f1\Editor\Unity.exe" -quit -batchmode -projectPath %cd%\ -executeMethod BuildMyGame.BuildAndroid -logFile %cd%\output\build.log %cd%\output\Minesweeper.apk

if %errorlevel% NEQ 0 (
  set /A exitCode=%errorlevel%
  echo Exit Code: %errorlevel%
  echo Build failed with exit code %exitCode%
)

robocopy %cd%\output\ d:\output\ /MIR

echo Build ended (exit code %exitCode%)...

exit /b %exitCode%
