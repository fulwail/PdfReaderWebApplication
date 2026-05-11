FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PdfReader.Worker/PdfReader.Worker.csproj", "PdfReader.Worker/"]
RUN dotnet restore "PdfReader.Worker/PdfReader.Worker.csproj"
COPY . .
WORKDIR "/src/PdfReader.Worker"
RUN dotnet build "./PdfReader.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PdfReader.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PdfReader.Worker.dll"]
