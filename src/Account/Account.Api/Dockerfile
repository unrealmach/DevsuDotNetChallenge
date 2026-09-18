# Contexto de build: la raiz del repositorio (ver docker-compose.yml)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Los csproj primero: asi el restore se cachea mientras no cambien las dependencias.
COPY Directory.Build.props ./
COPY src/Account/Account.Domain/Account.Domain.csproj             src/Account/Account.Domain/
COPY src/Account/Account.Application/Account.Application.csproj   src/Account/Account.Application/
COPY src/Account/Account.Infrastructure/Account.Infrastructure.csproj src/Account/Account.Infrastructure/
COPY src/Account/Account.Api/Account.Api.csproj                   src/Account/Account.Api/
RUN dotnet restore src/Account/Account.Api/Account.Api.csproj

COPY src/Account/ src/Account/
RUN dotnet publish src/Account/Account.Api/Account.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Account.Api.dll"]
