echo Start building...

"C:\Program Files\Unity\Hub\Editor\2019.4.18f1\Editor\Unity.exe" -quit -batchmode -projectPath . -logFile ./Logs/Builds/build.log ./Minesweeper.apk

TYPE ./Logs/Builds/build.log

rem -executeMethod BuildMyGame.BuildAndroid
echo Build ended...
