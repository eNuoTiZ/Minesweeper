#! /bin/sh

echo Start building...

ls -lh

'/Applications/Unity/Hub/Editor/2019.4.18f1/Unity.app/Contents/MacOS/Unity' -quit -batchmode -projectPath . -executeMethod BuildMyGame.BuildIos -logFile ./Logs/Builds/build.log


echo Build ended...
