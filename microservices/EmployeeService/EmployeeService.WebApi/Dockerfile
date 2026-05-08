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
COPY ["EmployeeService/EmployeeService.WebApi/EmployeeService.WebApi.csproj", "EmployeeService/EmployeeService.WebApi/"]
COPY ["EmployeeService/EmployeeService.Business/EmployeeService.Business.csproj", "EmployeeService/EmployeeService.Business/"]
COPY ["EmployeeService/EmployeeService.Repository/EmployeeService.Repository.csproj", "EmployeeService/EmployeeService.Repository/"]
COPY ["EmployeeService/EmployeeService.Shared/EmployeeService.Shared.csproj", "EmployeeService/EmployeeService.Shared/"]
COPY ["EmployeeService/EmployeeService.ClientHttp/EmployeeService.ClientHttp.csproj", "EmployeeService/EmployeeService.ClientHttp/"]

# Esegue il restore globale
RUN dotnet restore "EmployeeService/EmployeeService.WebApi/EmployeeService.WebApi.csproj"

# Ora copia tutto il resto del codice sorgente
COPY ["EmployeeService/", "EmployeeService/"]

# Spostati nella cartella del progetto principale
WORKDIR "/src/EmployeeService/EmployeeService.WebApi"

# Compila
RUN dotnet build "EmployeeService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EmployeeService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeService.WebApi.dll"]