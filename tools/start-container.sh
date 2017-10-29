if [ ! "$(docker ps -q -f name=workscheduleexport)" ]; then
    if [ "$(docker ps -aq -f status=exited -f name=workscheduleexport)" ]; then
        docker rm -f workscheduleexport
    fi
    docker run -d -p 5000:5000 --name workscheduleexport --restart=always -e BUILD=$BUILD_ID -e ASPNETCORE_URLS=$HOST_ADDRESS $DOCKER_REGISTRY_URL/workscheduleexport:$BUILD_ID
fi