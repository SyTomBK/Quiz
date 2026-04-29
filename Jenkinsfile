pipeline {
		agent any
		environment {
			gitRepository_source = 'https://gitlab.eztek.net/trueconnect/be_agency/services/quizv2'
			gitBranch_source = 'develop'
			gitlabCredential = 'gitlab_jenkin'

			// gitRepository_manifest = 'https://gitlab.eztek.net/kubernetes/helm.git'
			// gitBranch_manifest = 'main'
			// fileDeployement = "Agency/Contractv2/values.yaml"

			dockerregistry = 'https://registry.eztek.net'
			registryCredential = 'nexus_jenkin'

			dockerimagename = "registry.eztek.net/agency/quizv2-svc"
			// dockerimage_tag = "registry.eztek.net/${dockerimagename}"
			version = "stable-0.${BUILD_NUMBER}"
			DOCKERFILE = "./Dockerfile"
			context = "."

			BOT_TOKEN = credentials('TELEGRAM_BOT_TOKEN1')
			CHAT_ID = '-1003095283377'
		}

		stages {
			stage('Checkout project')
			{
				steps {
					git branch: gitBranch_source,
				    credentialsId: gitlabCredential,
				    url: gitRepository_source
					sh "git reset --hard"
				}
			}

			stage('Build docker')
			{
			  	agent any
			  	steps {
					script {
						try {
								sh "pwd"
								def dockerImage = docker.build("${dockerimagename}:${version}", "-f \$DOCKERFILE $context")
							}
						catch (Exception e) {
								sh 'curl -s -X POST https://api.telegram.org/$BOT_TOKEN/sendMessage -d chat_id=$CHAT_ID -d text="[Build]BE-quizv2-svc: FAILED"'
								error("Build Docker thất bại")
							}
						}
					}
			}


			stage('Pushing Image')
			{
      			steps {
        			script {
           				docker.withRegistry( dockerregistry , registryCredential ) {
							sh "docker push ${dockerimagename}:${version}"
        					sh "docker tag ${dockerimagename}:${version} ${dockerimagename}:latest"
        					sh "docker push ${dockerimagename}:latest"
							sh "docker rmi ${dockerimagename}:latest -f || true"
							sh "docker rmi ${dockerimagename}:${version} -f || true"
          				}
          			}
        	  	}
    	    }

			stage('Deploy')
			{
				steps {
					script {
						sh "pwd"
						sh "ssh root@10.10.10.76 docker compose -f /var/www/html/Agency/Services/quizv2-svc/docker-compose.yml down"
						sh "ssh root@10.10.10.76 docker rmi registry.eztek.net/agency/quizv2-svc:latest -f || true"
						sh "ssh root@10.10.10.76 docker compose -f /var/www/html/Agency/Services/quizv2-svc/docker-compose.yml up -d"
						sh 'curl -s -X POST https://api.telegram.org/$BOT_TOKEN/sendMessage -d chat_id=$CHAT_ID -d text="[BUILD]BE-quizv2-svc: Successful"'
					}
				}
            }
		}
	}
