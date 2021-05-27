FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS installer-env

WORKDIR /src/
COPY . ./

RUN dotnet restore 
RUN dotnet publish *.csproj -o out --no-restore

FROM mcr.microsoft.com/azure-functions/dotnet:3.0

ENV AzureWebJobsScriptRoot=/app/
ENV AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["src/out/", "/app/"]

RUN apt-get update && apt-get install -y curl
HEALTHCHECK CMD curl --fail http://localhost:80/api/health

EXPOSE 80