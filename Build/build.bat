@echo off
SETLOCAL EnableDelayedExpansion

echo Start building...

if not exist %cd%\output\ (
  echo Creating output directory
  mkdir %cd%\output\
)


"C:\Program Files\Unity\Hub\Editor\2019.4.18f1\Editor\Unity.exe" -quit -batchmode -projectPath %cd%\ -logFile %cd%\output\build.log %cd%\output\Minesweeper.apk
::-executeMethod BuildMyGame.BuildAndroid

if %errorlevel% NEQ 0 (
  set /A exitCode=%errorlevel%
  echo Exit Code: %errorlevel%
  echo Build failed with exit code %exitCode%
)

if exist D:\output (
  rmdir /S /Q D:\output
)

mkdir D:\output

copy %cd%\output\*.* D:\output
::GitHub\Minesweeper


echo Build ended (exit code %exitCode%)...

exit /b %exitCode%
