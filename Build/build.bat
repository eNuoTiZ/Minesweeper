@echo off
SETLOCAL EnableDelayedExpansion

echo Start building...

if not exist .\output (
  mkdir .\output
)

::set /A exitCode=0

"C:\Program Files\Unity\Hub\Editor\2019.4.18f1\Editor\Unity.exe" -quit -batchmode -projectPath . -executeMethod BuildMyGame.BuildAndroid -logFile .\output\build.log .\output\Minesweeper.apk

if %errorlevel% NEQ 0 (
  set /A exitCode=%errorlevel%
  echo Exit Code: %errorlevel%
  echo Build failed with exit code %exitCode%
)

if exist D:\GitHub\Minesweeper\output (
  rmdir /S /Q D:\GitHub\Minesweeper\output
)

copy .\output\*.* D:\GitHub\Minesweeper\output

echo Build ended (exit code %exitCode%)...

exit /b %exitCode%
