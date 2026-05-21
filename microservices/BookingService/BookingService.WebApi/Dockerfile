# 1. Base Runtime: Immagine leggera per l'esecuzione
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Build Stage: Immagine con SDK per compilare il codice
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

# DICHIARAZIONE ARGOMENTO PER IL TOKEN (Necessario per GitHub Packages)
ARG NUGET_AUTH_TOKEN 
ENV NUGET_AUTH_TOKEN=$NUGET_AUTH_TOKEN

WORKDIR /src

# Copia il file nuget.config per gestire i feed privati
COPY ["nuget.config", "./"]

# Copia i file di progetto (.csproj) per ottimizzare la cache dei layer Docker
COPY ["microservices/BookingService/BookingService.WebApi/BookingService.WebApi.csproj", "microservices/BookingService/BookingService.WebApi/"]
COPY ["microservices/BookingService/BookingService.Business/BookingService.Business.csproj", "microservices/BookingService/BookingService.Business/"]
COPY ["microservices/BookingService/BookingService.Repository/BookingService.Repository.csproj", "microservices/BookingService/BookingService.Repository/"]
COPY ["microservices/BookingService/BookingService.Shared/BookingService.Shared.csproj", "microservices/BookingService/BookingService.Shared/"]
COPY ["microservices/BookingService/BookingService.ClientHttp/BookingService.ClientHttp.csproj", "microservices/BookingService/BookingService.ClientHttp/"]

# Esegue il restore utilizzando il token e il file di configurazione specifico
RUN dotnet restore "microservices/BookingService/BookingService.WebApi/BookingService.WebApi.csproj" \
    --configfile nuget.config

# Copia tutto il resto del codice sorgente
COPY ["microservices/BookingService/", "microservices/BookingService/"]

# Si sposta nella cartella del progetto principale per la compilazione
WORKDIR "/src/microservices/BookingService/BookingService.WebApi"

# Compila l'applicazione
RUN dotnet build "BookingService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish Stage: Prepara i file ottimizzati per la distribuzione
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookingService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final Stage: Crea l'immagine finale basata sul runtime
FROM base AS final
WORKDIR /app
# Copia i file compilati dallo stage di pubblicazione
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingService.WebApi.dll"]