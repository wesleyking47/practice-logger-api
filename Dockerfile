# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# copy sln and csproj files, restore as distinct layer
COPY PracticeLogger.sln ./
COPY src/PracticeLogger.Api/PracticeLogger.Api.csproj src/PracticeLogger.Api/
COPY src/PracticeLogger.Application/PracticeLogger.Application.csproj src/PracticeLogger.Application/
COPY src/PracticeLogger.Domain/PracticeLogger.Domain.csproj src/PracticeLogger.Domain/
COPY src/PracticeLogger.Infrastructure/PracticeLogger.Infrastructure.csproj src/PracticeLogger.Infrastructure/
RUN dotnet restore src/PracticeLogger.Api/PracticeLogger.Api.csproj

# copy everything else and publish
COPY src/ ./src/
WORKDIR /source/src/PracticeLogger.Api
RUN dotnet publish -c Release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "PracticeLogger.Api.dll"]
