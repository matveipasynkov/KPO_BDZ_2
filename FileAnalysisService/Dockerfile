FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 6002

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY ["FileAnalysisService/FileAnalysisService.csproj", "FileAnalysisService/"]
RUN dotnet restore "FileAnalysisService/FileAnalysisService.csproj"
COPY . .
WORKDIR "/src/FileAnalysisService"
RUN dotnet publish "FileAnalysisService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FileAnalysisService.dll"] 