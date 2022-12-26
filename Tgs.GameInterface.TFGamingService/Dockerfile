#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.TFGamingService/Tgs.GameInterface.TFGamingService.csproj", "Tgs.GameInterface.TFGamingService/"]
COPY ["Tgs.GameInterface.TFGamingService.Core/Tgs.GameInterface.TFGamingService.Core.csproj", "Tgs.GameInterface.TFGamingService.Core/"]
RUN dotnet restore "Tgs.GameInterface.TFGamingService/Tgs.GameInterface.TFGamingService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.TFGamingService"
RUN dotnet build "Tgs.GameInterface.TFGamingService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameInterface.TFGamingService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.TFGamingService.dll"]