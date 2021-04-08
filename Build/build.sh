#! /bin/sh

echo Start building...

outputDir=backup_dir=$(date +'%m%d%Y-%H%M%S')

mkdir /Documents/BuildLogs/$outputDir/

'/Applications/Unity/Hub/Editor/2019.4.18f1/Unity.app/Contents/MacOS/Unity' -quit -batchmode -projectPath . -executeMethod BuildMyGame.BuildIos -logFile /Documents/BuildLogs/$outputDir/build.log

exitCode=$?

if [ $exitCode -eq 0 ]
then
  echo "Build succeeded!"
  exit 0
else
  echo "Build failed, check log for more information." >&2
  exit $exitCode
fi

echo Build ended...
