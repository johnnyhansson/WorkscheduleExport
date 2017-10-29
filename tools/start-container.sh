if [ ! "$(docker ps -q -f name=workscheduleexport)" ]; then
    if [ "$(docker ps -aq -f status=exited -f name=workscheduleexport)" ]; then
        docker rm -f workscheduleexport
    fi
    docker run -d -p 5000:5000 --name workscheduleexport --restart=always -e BUILD='${env.BUILD_ID}' -e ASPNETCORE_URLS='${params.HOST_ADDRESS}'${params.DOCKER_REGISTRY_URL}/workscheduleexport:${env.BUILD_ID}
fi