echo Start building...

"C:\Program Files\Unity\Hub\Editor\2019.4.18f1\Editor\Unity.exe" -quit -batchmode -projectPath . -executeMethod BuildMyGame.BuildAndroid -logFile .\Logs\Builds\build.log .\Minesweeper.apk

type .\Logs\Builds\build.log

copy .\Minesweeper.apk D:\GitHub\Minesweeper\Minesweeper.apk

echo Build ended...
