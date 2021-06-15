FROM mcr.microsoft.com/dotnet/sdk:3.1-buster AS build
WORKDIR /source

ARG TARGETPLATFORM
ARG FEED_ACCESSTOKEN

COPY . .

ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://johnnyhansson.pkgs.visualstudio.com/_packaging/Global/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"
ENV DOCKER "true"

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | bash

RUN dotnet publish -c release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim-arm32v7
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "./WorkScheduleExport.Web.dll"]