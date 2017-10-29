if [ ! "$(docker ps -q -f name=workschedule)" ]; then
    if [ "$(docker ps -aq -f status=exited -f name=workschedule)" ]; then
        docker rm -f workschedule
    fi
    docker run -d -p 5000:5000 --name workscheduleexport --restart=always -e BUILD='${env.BUILD_ID}' -e ASPNETCORE_URLS='${params.HOST_ADDRESS}'${params.DOCKER_REGISTRY_URL}/workscheduleexport:${env.BUILD_ID}
fi