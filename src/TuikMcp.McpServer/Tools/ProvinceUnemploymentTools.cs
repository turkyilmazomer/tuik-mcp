using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using TuikMcp.Application.Services;

namespace TuikMcp.McpServer.Tools;

/// <summary>
/// TÜİK İl Bazında İşsizlik verileri için MCP Tool tanımları
/// </summary>
[McpServerToolType]
public class ProvinceUnemploymentTools
{
    private readonly ProvinceUnemploymentAppService _provinceUnemploymentService;

    public ProvinceUnemploymentTools(ProvinceUnemploymentAppService provinceUnemploymentService)
    {
        _provinceUnemploymentService = provinceUnemploymentService;
    }

    [McpServerTool(Name = "get_province_unemployment")]
    [Description("Belirtilen ilin işsizlik verilerini yıllara göre getirir. İşsizlik oranı, genç işsizlik, işgücüne katılım, istihdam oranı ve işsiz sayısını içerir.")]
    public async Task<string> GetProvinceUnemployment(
        [Description("İl adı (örn: İstanbul, Diyarbakır, Şanlıurfa, Van)")] string province,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _provinceUnemploymentService.GetByProvinceAsync(province, cancellationToken);

        if (!dataList.Any())
            return $"{province} iline ait işsizlik verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK İl Bazında İşsizlik - {province}");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList)
        {
            sb.AppendLine($"📅 {data.Year}:");
            sb.AppendLine($"   📉 İşsizlik Oranı: %{data.UnemploymentRate:F1}");
            sb.AppendLine($"   👶 Genç İşsizlik (15-24): %{data.YouthUnemploymentRate:F1}");
            sb.AppendLine($"   💼 İşgücüne Katılma: %{data.LabourForceParticipationRate:F1}");
            sb.AppendLine($"   🏢 İstihdam Oranı: %{data.EmploymentRate:F1}");
            sb.AppendLine($"   👥 İşgücü: {data.LabourForce:N0}");
            sb.AppendLine($"   👤 İşsiz Sayısı: {data.NumberOfUnemployed:N0}");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_province_unemployment_by_year")]
    [Description("Belirtilen yılda tüm illerin işsizlik verilerini getirir. İller arası işsizlik karşılaştırması için kullanılır. İşsizlik oranına göre sıralı gelir.")]
    public async Task<string> GetProvinceUnemploymentByYear(
        [Description("İşsizlik verilerinin istenen yılı (örn: 2023)")] int year,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _provinceUnemploymentService.GetByYearAsync(year, cancellationToken);

        if (!dataList.Any())
            return $"{year} yılına ait il bazında işsizlik verisi bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"📊 TÜİK İl Bazında İşsizlik - {year} (İşsizlik Oranına Göre Sıralı)");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        foreach (var data in dataList)
        {
            sb.AppendLine($"📍 {data.Province,-15} | İşsizlik %{data.UnemploymentRate:F1} | Genç %{data.YouthUnemploymentRate:F1} | Katılım %{data.LabourForceParticipationRate:F1} | İstihdam %{data.EmploymentRate:F1}");
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "get_province_unemployment_ranking")]
    [Description("Belirtilen yılda işsizlik oranı en yüksek illerin sıralamasını getirir. Hangi illerde işsizliğin en ciddi olduğunu görmek için kullanılır.")]
    public async Task<string> GetProvinceUnemploymentRanking(
        [Description("Sıralamanın istenen yılı (örn: 2023)")] int year,
        CancellationToken cancellationToken = default)
    {
        var dataList = await _provinceUnemploymentService.GetRankingByUnemploymentRateAsync(year, cancellationToken);

        if (!dataList.Any())
            return $"{year} yılına ait il bazında işsizlik sıralaması bulunamadı.";

        var sb = new StringBuilder();
        sb.AppendLine($"🔴 TÜİK İşsizlik Sıralaması - {year} (En Yüksekten En Düşüğe)");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        var rank = 1;
        foreach (var data in dataList)
        {
            var indicator = data.UnemploymentRate >= 15 ? "🔴" : data.UnemploymentRate >= 10 ? "🟡" : "🟢";
            sb.AppendLine($"{rank,2}. {indicator} {data.Province,-15} | İşsizlik %{data.UnemploymentRate:F1} | Genç %{data.YouthUnemploymentRate:F1} | İşsiz {data.NumberOfUnemployed:N0}");
            rank++;
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "compare_province_unemployment_education")]
    [Description("Bir ilin işsizlik ve eğitim verilerini bir arada getirir. İşsizlik ile eğitim seviyesi arasındaki ilişkiyi analiz etmek için kullanılır.")]
    public async Task<string> CompareProvinceUnemploymentAndEducation(
        [Description("İl adı (örn: Şanlıurfa, Diyarbakır, Ankara)")] string province,
        [Description("Karşılaştırma yılı (örn: 2023)")] int year,
        EducationAppService educationService,
        CancellationToken cancellationToken = default)
    {
        var unemploymentData = await _provinceUnemploymentService.GetByProvinceAndYearAsync(province, year, cancellationToken);
        var educationData = await educationService.GetByProvinceAndYearAsync(province, year, cancellationToken);

        var sb = new StringBuilder();
        sb.AppendLine($"📊 {province} - İşsizlik & Eğitim Analizi ({year})");
        sb.AppendLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        if (unemploymentData is not null)
        {
            sb.AppendLine($"📉 İŞSİZLİK VERİLERİ:");
            sb.AppendLine($"   İşsizlik Oranı: %{unemploymentData.UnemploymentRate:F1}");
            sb.AppendLine($"   Genç İşsizlik (15-24): %{unemploymentData.YouthUnemploymentRate:F1}");
            sb.AppendLine($"   İşgücüne Katılma: %{unemploymentData.LabourForceParticipationRate:F1}");
            sb.AppendLine($"   İstihdam Oranı: %{unemploymentData.EmploymentRate:F1}");
            sb.AppendLine($"   İşsiz Sayısı: {unemploymentData.NumberOfUnemployed:N0}");
        }
        else
        {
            sb.AppendLine($"⚠️ {province} için {year} yılı işsizlik verisi bulunamadı.");
        }

        sb.AppendLine();

        if (educationData is not null)
        {
            sb.AppendLine($"🎓 EĞİTİM VERİLERİ:");
            sb.AppendLine($"   Okuryazarlık Oranı: %{educationData.LiteracyRate:F1}");
            sb.AppendLine($"   Lise Mezun Oranı: %{educationData.HighSchoolRate:F1}");
            sb.AppendLine($"   Üniversite Mezun Oranı: %{educationData.UniversityRate:F1}");
            sb.AppendLine($"   Okuma Yazma Bilmeyen: {educationData.IlliteratePopulation:N0}");
            sb.AppendLine($"   Ortalama Eğitim Süresi: {educationData.AverageSchoolingYears:F1} yıl");
        }
        else
        {
            sb.AppendLine($"⚠️ {province} için {year} yılı eğitim verisi bulunamadı.");
        }

        if (unemploymentData is not null && educationData is not null)
        {
            sb.AppendLine();
            sb.AppendLine($"📈 ÖZET ANALİZ:");
            var educationLevel = educationData.UniversityRate >= 25 ? "yüksek" : educationData.UniversityRate >= 18 ? "orta" : "düşük";
            var unemploymentLevel = unemploymentData.UnemploymentRate >= 15 ? "yüksek" : unemploymentData.UnemploymentRate >= 10 ? "orta" : "düşük";
            sb.AppendLine($"   Eğitim Seviyesi: {educationLevel} (Üniversite %{educationData.UniversityRate:F1})");
            sb.AppendLine($"   İşsizlik Seviyesi: {unemploymentLevel} (%{unemploymentData.UnemploymentRate:F1})");
        }

        return sb.ToString();
    }
}
