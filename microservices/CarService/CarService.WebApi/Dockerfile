# 1. Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia dei file .csproj mantenendo la struttura delle cartelle 
COPY ["microservices/CarService/CarService.WebApi/CarService.WebApi.csproj", "microservices/CarService/CarService.WebApi/"]
COPY ["microservices/CarService/CarService.Business/CarService.Business.csproj", "microservices/CarService/CarService.Business/"]
COPY ["microservices/CarService/CarService.Repository/CarService.Repository.csproj", "microservices/CarService/CarService.Repository/"]
COPY ["microservices/CarService/CarService.Shared/CarService.Shared.csproj", "microservices/CarService/CarService.Shared/"]
COPY ["microservices/CarService/CarService.ClientHttp/CarService.ClientHttp.csproj", "microservices/CarService/CarService.ClientHttp/"]

# Esegue il restore puntando al file specifico [cite: 9]
RUN dotnet restore "microservices/CarService/CarService.WebApi/CarService.WebApi.csproj"

# Copia tutto il resto del codice della cartella CarService 
COPY ["microservices/CarService/", "microservices/CarService/"]

# COMPILA stando in /src per non rompere i riferimenti relativi tra i progetti
RUN dotnet build "microservices/CarService/CarService.WebApi/CarService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "microservices/CarService/CarService.WebApi/CarService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarService.WebApi.dll"]