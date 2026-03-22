using TuikMcp.Domain.Entities;
using TuikMcp.Domain.Interfaces;
using TuikMcp.Infrastructure.Data;

namespace TuikMcp.Infrastructure.Services;

/// <summary>
/// Gömülü JSON verilerinden il bazında işsizlik istatistiklerini sunan servis.
/// Veri kaynağı: TÜİK Bölgesel İşgücü İstatistikleri
/// </summary>
public class TuikProvinceUnemploymentService : IProvinceUnemploymentService
{
    private static readonly Lazy<List<ProvinceUnemploymentData>> _data = new(
        () => EmbeddedJsonDataProvider.Load<ProvinceUnemploymentData>("province_unemployment.json"));

    private static List<ProvinceUnemploymentData> Data => _data.Value;

    public Task<IReadOnlyList<ProvinceUnemploymentData>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ProvinceUnemploymentData> result = Data
            .Where(p => string.Equals(p.Province, province, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(p => p.Year)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<ProvinceUnemploymentData>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ProvinceUnemploymentData> result = Data
            .Where(p => p.Year == year)
            .OrderBy(p => p.UnemploymentRate)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<ProvinceUnemploymentData>> GetRankingByUnemploymentRateAsync(int year, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ProvinceUnemploymentData> result = Data
            .Where(p => p.Year == year && p.Province != "Türkiye")
            .OrderByDescending(p => p.UnemploymentRate)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<ProvinceUnemploymentData?> GetByProvinceAndYearAsync(string province, int year, CancellationToken cancellationToken = default)
    {
        var result = Data.FirstOrDefault(p =>
            string.Equals(p.Province, province, StringComparison.OrdinalIgnoreCase) && p.Year == year);

        return Task.FromResult(result);
    }
}
