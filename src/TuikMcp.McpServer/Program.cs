using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using global::TuikMcp.Application.DependencyInjection;
using global::TuikMcp.Infrastructure.DependencyInjection;
using global::TuikMcp.McpServer.Tools;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Information;
});

// Clean Architecture katman servisleri
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// MCP Server yapılandırması - Streamable HTTP Transport
builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new()
        {
            Name = "TuikMcp",
            Version = "1.0.0"        };        options.ServerInstructions = """
            Bu MCP sunucusu TÜİK (Türkiye İstatistik Kurumu) verilerine erişim sağlar.
            Beş ana veri kategorisi sunulmaktadır:
            
            1. 📊 Nüfus Verileri: Türkiye ve il bazında nüfus bilgileri (toplam, erkek, kadın, artış hızı) — 2010-2024
            2. 📉 İşsizlik Verileri (Türkiye Geneli): Aylık işsizlik oranı, genç işsizlik, işgücüne katılım, istihdam oranı — 2020-2025
            3. 📈 Enflasyon (TÜFE) Verileri: Aylık/yıllık değişim, 12 aylık ortalama, tüketici fiyat endeksi — 2021-2025
            4. 🎓 Eğitim Verileri: İl bazında okuryazarlık, lise/üniversite mezun oranları, ortalama eğitim süresi — 2021-2023
            5. 🏙️ İl Bazında İşsizlik: İl bazında işsizlik oranları, genç işsizlik, işgücü katılım, eğitim-işsizlik karşılaştırması — 2021-2023
            
            Veriler TÜİK resmi yayınlarına dayalı gömülü veri setlerinden sunulmaktadır.
            İl bazında eğitim ve işsizlik verileri arasında çapraz analiz yapılabilir.
            """;
    })    .WithHttpTransport()
    .WithTools<PopulationTools>()
    .WithTools<UnemploymentTools>()
    .WithTools<InflationTools>()
    .WithTools<EducationTools>()
    .WithTools<ProvinceUnemploymentTools>();

var app = builder.Build();

// Health check endpoint (Docker HEALTHCHECK + load balancer probes)
app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "TuikMcp", version = "1.0.0" }));

app.MapMcp("/mcp");

app.Run();
