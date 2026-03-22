using TuikMcp.Domain.Entities;
using TuikMcp.Domain.Interfaces;
using TuikMcp.Infrastructure.Data;

namespace TuikMcp.Infrastructure.Services;

/// <summary>
/// Gömülü JSON verilerinden eğitim istatistiklerini sunan servis.
/// Veri kaynağı: TÜİK Eğitim İstatistikleri
/// </summary>
public class TuikEducationService : IEducationService
{
    private static readonly Lazy<List<EducationData>> _data = new(
        () => EmbeddedJsonDataProvider.Load<EducationData>("education.json"));

    private static List<EducationData> Data => _data.Value;

    public Task<IReadOnlyList<EducationData>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<EducationData> result = Data
            .Where(e => string.Equals(e.Province, province, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.Year)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<EducationData>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<EducationData> result = Data
            .Where(e => e.Year == year)
            .OrderByDescending(e => e.UniversityRate)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<EducationData?> GetByProvinceAndYearAsync(string province, int year, CancellationToken cancellationToken = default)
    {
        var result = Data.FirstOrDefault(e =>
            string.Equals(e.Province, province, StringComparison.OrdinalIgnoreCase) && e.Year == year);

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<EducationData>> GetRankingByUniversityRateAsync(int year, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<EducationData> result = Data
            .Where(e => e.Year == year && e.Province != "Türkiye")
            .OrderByDescending(e => e.UniversityRate)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }
}
