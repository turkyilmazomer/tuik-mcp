using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using TuikMcp.Application.Services;

namespace TuikMcp.McpServer.Tools;

/// <summary>
/// TÜİK Nüfus verileri için MCP Tool tanımları
/// </summary>
[McpServerToolType]
public class PopulationTools
{
    private readonly PopulationAppService _populationService;

    public PopulationTools(PopulationAppService populationService)
    {
        _populationService = populationService;
    }

    [McpServerTool(Name = "get_population_by_year")]
    [Description("Türkiye'nin belirtilen yıla ait nüfus verilerini getirir. Toplam nüfus, erkek/kadın nüfusu ve yıllık nüfus artış hızını içerir.")]
    public async Task<string> GetPopulationByYear(
        [Description("Nüfus verisinin istenen yılı (örn: 2023)")] int year,
        CancellationToken cancellationToken = default)
    {
        var data = await _populationService.GetPopulationByYearAsync(year, cancellationToken);

        if (data is null)
            return $"{year} yılına ait nüfus verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Nüfus Verileri - {data.Year}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        sb.AppendLine($"🏠 Bölge: {data.Province ?? "Türkiye"}");
        sb.AppendLine($"👥 Toplam Nüfus: {data.TotalPopulation:N0}");
        sb.AppendLine($"👨 Erkek Nüfus: {data.MalePopulation:N0}");
        sb.AppendLine($"👩 Kadın Nüfus: {data.FemalePopulation:N0}");
        sb.AppendLine($"📈 Yıllık Artış Hızı: ‰{data.AnnualGrowthRate:F2}");

        return sb.ToString();
    }

    [McpServerTool(Name = "get_population_by_year_range")]
    [Description("Türkiye'nin belirtilen yıl aralığındaki nüfus verilerini getirir. Nüfus değişim trendini görmek için kullanılır.")]
    public async Task<string> GetPopulationByYearRange(
        [Description("Başlangıç yılı (örn: 2018)")] int startYear,
        [Description("Bitiş yılı (örn: 2023)")] int endYear,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _populationService.GetPopulationByYearRangeAsync(startYear, endYear, cancellationToken);

        if (!dataList.Any())
            return $"{startYear}-{endYear} yıl aralığına ait nüfus verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Nüfus Verileri - {startYear} ile {endYear} arası");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList.OrderBy(d => d.Year))
        {
            sb.AppendLine($"📅 {data.Year}: Toplam {data.TotalPopulation:N0} | Erkek {data.MalePopulation:N0} | Kadın {data.FemalePopulation:N0} | Artış ‰{data.AnnualGrowthRate:F2}");
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_population_by_province")]
    [Description("Belirtilen ilin nüfus verilerini getirir. İl adı Türkçe olarak verilmelidir (örn: İstanbul, Ankara, İzmir).")]
    public async Task<string> GetPopulationByProvince(
        [Description("İl adı (örn: İstanbul, Ankara, İzmir)")] string province,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _populationService.GetPopulationByProvinceAsync(province, cancellationToken);

        if (!dataList.Any())
            return $"{province} iline ait nüfus verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK Nüfus Verileri - {province}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList.OrderByDescending(d => d.Year))
        {
            sb.AppendLine($"📅 {data.Year}: Toplam {data.TotalPopulation:N0} | Erkek {data.MalePopulation:N0} | Kadın {data.FemalePopulation:N0}");
        }

        return sb.ToString();
    }
}
