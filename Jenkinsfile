pipeline {
  agent {
    node {
      label 'ios'
    }

  }
  stages {
    stage('Buid iOS') {
      steps {
        sh '\'/Applications/Unity/Hub/Editor/2019.4.18f1/Unity.app/Contents/MacOS/Unity\' -quit -batchmode -projectPath "${WORKSPACE}" -executeMethod BuildMyGame.BuildIos -logFile "${WORKSPACE}/Logs/Builds/build_ios.log"'
      }
    }

  }
}