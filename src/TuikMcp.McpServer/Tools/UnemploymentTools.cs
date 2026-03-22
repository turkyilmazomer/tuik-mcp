using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using TuikMcp.Application.Services;

namespace TuikMcp.McpServer.Tools;

/// <summary>
/// TÜİK İşsizlik verileri için MCP Tool tanımları
/// </summary>
[McpServerToolType]
public class UnemploymentTools
{
    private readonly UnemploymentAppService _unemploymentService;

    public UnemploymentTools(UnemploymentAppService unemploymentService)
    {
        _unemploymentService = unemploymentService;
    }

    [McpServerTool(Name = "get_unemployment_by_period")]
    [Description("Türkiye'nin belirtilen yıl ve aya ait işsizlik verilerini getirir. İşsizlik oranı, genç işsizlik oranı, işgücüne katılma oranı ve istihdam oranını içerir.")]
    public async Task<string> GetUnemploymentByPeriod(
        [Description("İşsizlik verisinin istenen yılı (örn: 2024)")] int year,
        [Description("İşsizlik verisinin istenen ayı (1-12 arası, örn: 6)")] int month,
        CancellationToken cancellationToken = default)
    {
        var data = await _unemploymentService.GetUnemploymentByPeriodAsync(year, month, cancellationToken);

        if (data is null)
            return $"{year}/{month:D2} dönemine ait işsizlik verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK İşsizlik Verileri - {data.Period}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        sb.AppendLine($"📉 İşsizlik Oranı: %{data.UnemploymentRate:F1}");
        sb.AppendLine($"👶 Genç İşsizlik Oranı (15-24 yaş): %{data.YouthUnemploymentRate:F1}");
        sb.AppendLine($"💼 İşgücüne Katılma Oranı: %{data.LabourForceParticipationRate:F1}");
        sb.AppendLine($"🏢 İstihdam Oranı: %{data.EmploymentRate:F1}");
        sb.AppendLine($"👤 İşsiz Sayısı: {data.NumberOfUnemployed:N0}");

        return sb.ToString();
    }

    [McpServerTool(Name = "get_unemployment_by_year")]
    [Description("Türkiye'nin belirtilen yıla ait tüm aylık işsizlik verilerini getirir. Yıl içindeki işsizlik trendini görmek için kullanılır.")]
    public async Task<string> GetUnemploymentByYear(
        [Description("İşsizlik verilerinin istenen yılı (örn: 2024)")] int year,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _unemploymentService.GetUnemploymentByYearAsync(year, cancellationToken);

        if (!dataList.Any())
            return $"{year} yılına ait işsizlik verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK İşsizlik Verileri - {year} Yılı");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList.OrderBy(d => d.Month))
        {
            sb.AppendLine($"📅 {data.Period}: İşsizlik %{data.UnemploymentRate:F1} | Genç %{data.YouthUnemploymentRate:F1} | Katılım %{data.LabourForceParticipationRate:F1} | İstihdam %{data.EmploymentRate:F1}");
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_latest_unemployment")]
    [Description("Türkiye'nin en güncel işsizlik verilerini getirir. En son açıklanan dönemin işsizlik oranı, genç işsizlik oranı ve diğer göstergeleri içerir.")]
    public async Task<string> GetLatestUnemployment(CancellationToken cancellationToken = default)
    {
        var data = await _unemploymentService.GetLatestUnemploymentAsync(cancellationToken);

        if (data is null)
            return "Güncel işsizlik verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Güncel İşsizlik Verileri - {data.Period}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        sb.AppendLine($"📉 İşsizlik Oranı: %{data.UnemploymentRate:F1}");
        sb.AppendLine($"👶 Genç İşsizlik Oranı (15-24 yaş): %{data.YouthUnemploymentRate:F1}");
        sb.AppendLine($"💼 İşgücüne Katılma Oranı: %{data.LabourForceParticipationRate:F1}");
        sb.AppendLine($"🏢 İstihdam Oranı: %{data.EmploymentRate:F1}");
        sb.AppendLine($"👤 İşsiz Sayısı: {data.NumberOfUnemployed:N0}");

        return sb.ToString();
    }
}
