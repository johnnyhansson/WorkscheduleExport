pipeline {
    agent any

    environment {
        CONFIGURATION = "Release"
    }

    stages {
        stage("Checkout") {
            steps {
                checkout scm
            }
        }

        stage("Build") {
            steps {
                bat "dotnet restore"
                bat "dotnet build -c %CONFIGURATION%"
            }
        }

        stage("Test") {
            steps {
                dir("WorkScheduleExport.Web.UnitTests") {
                    bat "dotnet xunit -configuration %CONFIGURATION% -nobuild -xml unittests.xml"
                }
            }
        }
    }

    post {
        always {
            step([$class: "XUnitBuilder", thresholds: [[$class: "FailedThreshold", failureThreshold: "0"]], 
                tools: [[$class: "CustomType", deleteOutputFiles: false, pattern: "**/*.xml", customXSL: "tools/xunitdotnet-2.0-to-junit-2.xsl"]]])

            cleanWs()
        }
    }
}