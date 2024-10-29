#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Vendas.API/Vendas.API.csproj", "src/Vendas.API/"]
COPY ["src/Vendas.Core/Vendas.Core.csproj", "src/Vendas.Core/"]
COPY ["src/Vendas.Data/Vendas.Data.csproj", "src/Vendas.Data/"]
COPY ["src/Vendas.Domain/Vendas.Domain.csproj", "src/Vendas.Domain/"]
RUN dotnet restore "./src/Vendas.API/Vendas.API.csproj"
COPY . .
WORKDIR "/src/src/Vendas.API"
RUN dotnet build "./Vendas.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Vendas.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vendas.API.dll"]