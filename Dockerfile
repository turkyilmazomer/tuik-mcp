# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution ve proje dosyalarını kopyala
COPY TuikMcp.sln .
COPY src/TuikMcp.Domain/TuikMcp.Domain.csproj src/TuikMcp.Domain/
COPY src/TuikMcp.Application/TuikMcp.Application.csproj src/TuikMcp.Application/
COPY src/TuikMcp.Infrastructure/TuikMcp.Infrastructure.csproj src/TuikMcp.Infrastructure/
COPY src/TuikMcp.McpServer/TuikMcp.McpServer.csproj src/TuikMcp.McpServer/

# NuGet restore
RUN dotnet restore TuikMcp.sln

# Tüm kaynak kodu kopyala
COPY . .

# Publish
RUN dotnet publish src/TuikMcp.McpServer/TuikMcp.McpServer.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Sağlık kontrolü için curl ekle
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Port ayarı (Railway, Fly.io, Cloud Run PORT env variable kullanır)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Sağlık kontrolü
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TuikMcp.McpServer.dll"]
