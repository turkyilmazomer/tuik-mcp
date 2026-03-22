using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using TuikMcp.Application.Services;

namespace TuikMcp.McpServer.Tools;

/// <summary>
/// TÜİK Enflasyon (TÜFE) verileri için MCP Tool tanımları
/// </summary>
[McpServerToolType]
public class InflationTools
{
    private readonly InflationAppService _inflationService;

    public InflationTools(InflationAppService inflationService)
    {
        _inflationService = inflationService;
    }

    [McpServerTool(Name = "get_inflation_by_period")]
    [Description("Türkiye'nin belirtilen yıl ve aya ait enflasyon (TÜFE) verilerini getirir. Aylık değişim, yıllık değişim, 12 aylık ortalama ve tüketici fiyat endeksini içerir.")]
    public async Task<string> GetInflationByPeriod(
        [Description("Enflasyon verisinin istenen yılı (örn: 2024)")] int year,
        [Description("Enflasyon verisinin istenen ayı (1-12 arası, örn: 6)")] int month,
        CancellationToken cancellationToken = default)
    {
        var data = await _inflationService.GetInflationByPeriodAsync(year, month, cancellationToken);

        if (data is null)
            return $"{year}/{month:D2} dönemine ait enflasyon verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Enflasyon (TÜFE) Verileri - {data.Period}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        sb.AppendLine($"📅 Aylık Değişim: %{data.MonthlyChange:F2}");
        sb.AppendLine($"📈 Yıllık Değişim: %{data.AnnualChange:F2}");
        sb.AppendLine($"📊 12 Aylık Ortalama Değişim: %{data.TwelveMonthAverage:F2}");
        sb.AppendLine($"🔢 Tüketici Fiyat Endeksi: {data.ConsumerPriceIndex:F2}");

        return sb.ToString();
    }

    [McpServerTool(Name = "get_inflation_by_year")]
    [Description("Türkiye'nin belirtilen yıla ait tüm aylık enflasyon (TÜFE) verilerini getirir. Yıl içindeki enflasyon trendini görmek için kullanılır.")]
    public async Task<string> GetInflationByYear(
        [Description("Enflasyon verilerinin istenen yılı (örn: 2024)")] int year,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _inflationService.GetInflationByYearAsync(year, cancellationToken);

        if (!dataList.Any())
            return $"{year} yılına ait enflasyon verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Enflasyon (TÜFE) Verileri - {year} Yılı");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList.OrderBy(d => d.Month))
        {
            sb.AppendLine($"📅 {data.Period}: Aylık %{data.MonthlyChange:F2} | Yıllık %{data.AnnualChange:F2} | 12 Ay Ort. %{data.TwelveMonthAverage:F2} | Endeks {data.ConsumerPriceIndex:F2}");
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_latest_inflation")]
    [Description("Türkiye'nin en güncel enflasyon (TÜFE) verilerini getirir. En son açıklanan dönemin aylık değişim, yıllık değişim ve tüketici fiyat endeksini içerir.")]
    public async Task<string> GetLatestInflation(CancellationToken cancellationToken = default)
    {
        var data = await _inflationService.GetLatestInflationAsync(cancellationToken);

        if (data is null)
            return "Güncel enflasyon verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Güncel Enflasyon (TÜFE) Verileri - {data.Period}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        sb.AppendLine($"📅 Aylık Değişim: %{data.MonthlyChange:F2}");
        sb.AppendLine($"📈 Yıllık Değişim: %{data.AnnualChange:F2}");
        sb.AppendLine($"📊 12 Aylık Ortalama Değişim: %{data.TwelveMonthAverage:F2}");
        sb.AppendLine($"🔢 Tüketici Fiyat Endeksi: {data.ConsumerPriceIndex:F2}");

        return sb.ToString();
    }
}
