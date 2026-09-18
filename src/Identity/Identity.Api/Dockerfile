# Contexto de build: la raiz del repositorio (ver docker-compose.yml)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Los csproj primero: asi el restore se cachea mientras no cambien las dependencias.
COPY Directory.Build.props ./
COPY src/Identity/Identity.Domain/Identity.Domain.csproj             src/Identity/Identity.Domain/
COPY src/Identity/Identity.Application/Identity.Application.csproj   src/Identity/Identity.Application/
COPY src/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj src/Identity/Identity.Infrastructure/
COPY src/Identity/Identity.Api/Identity.Api.csproj                   src/Identity/Identity.Api/
RUN dotnet restore src/Identity/Identity.Api/Identity.Api.csproj

COPY src/Identity/ src/Identity/
RUN dotnet publish src/Identity/Identity.Api/Identity.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
