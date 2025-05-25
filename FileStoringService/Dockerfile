# Dockerfile для FileStoringService
# Использует .NET 9 (preview) для сборки и запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY ["FileStoringService/FileStoringService.csproj", "FileStoringService/"]
RUN dotnet restore "FileStoringService/FileStoringService.csproj"
COPY . .
WORKDIR "/src/FileStoringService"
RUN dotnet build "FileStoringService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileStoringService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStoringService.dll"] 