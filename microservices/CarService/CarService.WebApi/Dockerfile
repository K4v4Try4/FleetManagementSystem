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
# Modifica i percorsi per includere 'microservices/EmployeeService/'
COPY ["microservices/EmployeeService/EmployeeService.WebApi/EmployeeService.WebApi.csproj", "microservices/EmployeeService/EmployeeService.WebApi/"]
COPY ["microservices/EmployeeService/EmployeeService.Shared/EmployeeService.Shared.csproj", "microservices/EmployeeService/EmployeeService.Shared/"]
COPY ["microservices/EmployeeService/EmployeeService.Business/EmployeeService.Business.csproj", "microservices/EmployeeService/EmployeeService.Business/"]
COPY ["microservices/EmployeeService/EmployeeService.Repository/EmployeeService.Repository.csproj", "microservices/EmployeeService/EmployeeService.Repository/"]
COPY ["microservices/EmployeeService/EmployeeService.ClientHttp/EmployeeService.ClientHttp.csproj", "microservices/EmployeeService/EmployeeService.ClientHttp/"]

RUN dotnet restore "microservices/EmployeeService/EmployeeService.WebApi/EmployeeService.WebApi.csproj"
COPY . .
WORKDIR "/src/microservices/EmployeeService/EmployeeService.WebApi"
RUN dotnet build "EmployeeService.WebApi.csproj" -c Release -o /app/build

# 3. Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarService.WebApi.dll"]