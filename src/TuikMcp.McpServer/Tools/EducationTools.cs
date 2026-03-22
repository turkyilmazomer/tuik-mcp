using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using TuikMcp.Application.Services;

namespace TuikMcp.McpServer.Tools;

/// <summary>
/// TÜİK Eğitim verileri için MCP Tool tanımları
/// </summary>
[McpServerToolType]
public class EducationTools
{
    private readonly EducationAppService _educationService;

    public EducationTools(EducationAppService educationService)
    {
        _educationService = educationService;
    }

    [McpServerTool(Name = "get_education_by_province")]
    [Description("Belirtilen ilin eğitim istatistiklerini getirir. Okuryazarlık oranı, lise/üniversite mezun oranları, okuma yazma bilmeyen sayısı ve ortalama eğitim süresi bilgilerini içerir.")]
    public async Task<string> GetEducationByProvince(
        [Description("İl adı (örn: İstanbul, Ankara, Diyarbakır, Şanlıurfa)")] string province,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _educationService.GetByProvinceAsync(province, cancellationToken);

        if (!dataList.Any())
            return $"{province} iline ait eğitim verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"🎓 TÜİK Eğitim İstatistikleri - {province}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList)
        {
            sb.AppendLine($"📅 {data.Year}:");
            sb.AppendLine($"   📖 Okuryazarlık Oranı: %{data.LiteracyRate:F1}");
            sb.AppendLine($"   🏫 Lise Mezun Oranı: %{data.HighSchoolRate:F1}");
            sb.AppendLine($"   🎓 Üniversite Mezun Oranı: %{data.UniversityRate:F1}");
            sb.AppendLine($"   📚 Üniversite Mezunu: {data.UniversityGraduates:N0}");
            sb.AppendLine($"   🏢 Lise Mezunu: {data.HighSchoolGraduates:N0}");
            sb.AppendLine($"   📝 İlkokul Mezunu: {data.PrimarySchoolGraduates:N0}");
            sb.AppendLine($"   ❌ Okuma Yazma Bilmeyen: {data.IlliteratePopulation:N0}");
            sb.AppendLine($"   ⏱️ Ortalama Eğitim Süresi: {data.AverageSchoolingYears:F1} yıl");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_education_by_year")]
    [Description("Belirtilen yıla ait tüm illerin eğitim istatistiklerini getirir. İller arası eğitim seviyesi karşılaştırması için kullanılır.")]
    public async Task<string> GetEducationByYear(
        [Description("Eğitim verilerinin istenen yılı (örn: 2023)")] int year,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _educationService.GetByYearAsync(year, cancellationToken);

        if (!dataList.Any())
            return $"{year} yılına ait eğitim verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"🎓 TÜİK Eğitim İstatistikleri - {year} (Üniversite Mezun Oranına Göre Sıralı)");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList)
        {
            sb.AppendLine($"📍 {data.Province,-15} | Okuryazarlık %{data.LiteracyRate:F1} | Lise %{data.HighSchoolRate:F1} | Üniversite %{data.UniversityRate:F1} | Ort. Eğitim {data.AverageSchoolingYears:F1} yıl");
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_education_ranking")]
    [Description("Belirtilen yılda illerin üniversite mezun oranına göre sıralamasını getirir. Eğitim seviyesi en yüksek ve en düşük illeri görmek için kullanılır.")]
    public async Task<string> GetEducationRanking(
        [Description("Sıralamanın istenen yılı (örn: 2023)")] int year,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _educationService.GetRankingByUniversityRateAsync(year, cancellationToken);

        if (!dataList.Any())
            return $"{year} yılına ait eğitim sıralaması bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"🏆 TÜİK Eğitim Sıralaması - {year} (Üniversite Mezun Oranına Göre)");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        var rank = 1;
        foreach (var data in dataList)
        {
            var medal = rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"{rank,2}." };
            sb.AppendLine($"{medal} {data.Province,-15} | Üniversite %{data.UniversityRate:F1} | Okuryazarlık %{data.LiteracyRate:F1} | Ort. Eğitim {data.AverageSchoolingYears:F1} yıl");
            rank++;
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_education_comparison")]
    [Description("İki il arasında eğitim seviyesi karşılaştırması yapar. Belirtilen yılda iki ilin okuryazarlık, lise, üniversite oranlarını yan yana gösterir.")]
    public async Task<string> GetEducationComparison(
        [Description("Birinci il adı (örn: Ankara)")] string province1,
        [Description("İkinci il adı (örn: Şanlıurfa)")] string province2,
        [Description("Karşılaştırma yılı (örn: 2023)")] int year,
        CancellationToken cancellationToken = default)
    {
        var data1 = await _educationService.GetByProvinceAndYearAsync(province1, year, cancellationToken);
        var data2 = await _educationService.GetByProvinceAndYearAsync(province2, year, cancellationToken);

        if (data1 is null && data2 is null)
            return $"{year} yılında {province1} ve {province2} için eğitim verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"🎓 Eğitim Karşılaştırması - {year}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        sb.AppendLine($"{"Gösterge",-30} | {province1,-15} | {province2,-15}");
        sb.AppendLine($"{"─────────────────────────────",-30} | {"───────────────",-15} | {"───────────────",-15}");

        if (data1 is not null && data2 is not null)
        {
            sb.AppendLine($"{"Okuryazarlık Oranı",-30} | %{data1.LiteracyRate,-14:F1} | %{data2.LiteracyRate,-14:F1}");
            sb.AppendLine($"{"Lise Mezun Oranı",-30} | %{data1.HighSchoolRate,-14:F1} | %{data2.HighSchoolRate,-14:F1}");
            sb.AppendLine($"{"Üniversite Mezun Oranı",-30} | %{data1.UniversityRate,-14:F1} | %{data2.UniversityRate,-14:F1}");
            sb.AppendLine($"{"Üniversite Mezunu Sayısı",-30} | {data1.UniversityGraduates,-15:N0} | {data2.UniversityGraduates,-15:N0}");
            sb.AppendLine($"{"Okuma Yazma Bilmeyen",-30} | {data1.IlliteratePopulation,-15:N0} | {data2.IlliteratePopulation,-15:N0}");
            sb.AppendLine($"{"Ortalama Eğitim Süresi",-30} | {data1.AverageSchoolingYears,-15:F1} | {data2.AverageSchoolingYears,-15:F1}");
        }
        else
        {
            if (data1 is null) sb.AppendLine($"⚠️ {province1} için {year} yılı verisi bulunamadı.");
            if (data2 is null) sb.AppendLine($"⚠️ {province2} için {year} yılı verisi bulunamadı.");
        }

        return sb.ToString();
    }
}
