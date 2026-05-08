# 1. Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Questo permette al restore di vedere i riferimenti tra i progetti
COPY ["CarService/CarService.WebApi/CarService.WebApi.csproj", "CarService/CarService.WebApi/"]
COPY ["CarService/CarService.Business/CarService.Business.csproj", "CarService/CarService.Business/"]
COPY ["CarService/CarService.Repository/CarService.Repository.csproj", "CarService/CarService.Repository/"]
COPY ["CarService/CarService.Shared/CarService.Shared.csproj", "CarService/CarService.Shared/"]
COPY ["CarService/CarService.ClientHttp/CarService.ClientHttp.csproj", "CarService/CarService.ClientHttp/"]

# Esegue il restore globale
RUN dotnet restore "CarService/CarService.WebApi/CarService.WebApi.csproj"

# Ora copia tutto il resto del codice sorgente
COPY ["CarService/", "CarService/"]

# Spostati nella cartella del progetto principale
WORKDIR "/src/CarService/CarService.WebApi"

# Compila
RUN dotnet build "CarService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarService.WebApi.dll"]