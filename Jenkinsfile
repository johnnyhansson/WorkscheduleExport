node {
	try {
		checkout scm

		stage("Build") {
			bat "dotnet build -c ${params.CONFIGURATION}"
			
			dir("WorkScheduleExport.Web") {
				bat "dotnet publish -c ${params.CONFIGURATION} -r linux-arm -o publish"
			}

			stash name: "binaries", includes: "WorkScheduleExport.Web/dockerfile.raspberrypi,WorkScheduleExport.Web/publish/,tools/start-container.sh"
		}

		stage("Test") {
			dir("WorkScheduleExport.Web.UnitTests") {
				bat "dotnet xunit -configuration ${params.CONFIGURATION} -nobuild -xml unittests.xml"
			}
		}

		stage("Deploy") {
			node("Arm") {
				unstash "binaries"

				dir("WorkScheduleExport.Web") {
					sh "mv dockerfile.raspberrypi Dockerfile"
					docker.withRegistry("${params.DOCKER_REGISTRY_URL}", "${params.DOCKER_REGISTRY_CREDENTIAL_ID}") {
						def image = docker.build("workscheduleexport:${env.BUILD_ID}")
						image.push()
					}
				}
				sh "chmod +x tools/start-container.sh"
				sh "tools/start-container.sh"
				cleanWs()
			}
		}
    }
    finally {
        step([$class: "XUnitBuilder", thresholds: [[$class: "FailedThreshold", failureThreshold: "0"]], 
                tools: [[$class: "CustomType", deleteOutputFiles: false, pattern: "**/*.xml", customXSL: "tools/xunitdotnet-2.0-to-junit-2.xsl"]]])

        cleanWs()
    }
}