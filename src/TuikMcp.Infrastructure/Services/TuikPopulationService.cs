using TuikMcp.Domain.Entities;
using TuikMcp.Domain.Interfaces;
using TuikMcp.Infrastructure.Data;

namespace TuikMcp.Infrastructure.Services;

/// <summary>
/// Gömülü JSON verilerinden nüfus istatistiklerini sunan servis.
/// Veri kaynağı: TÜİK Adrese Dayalı Nüfus Kayıt Sistemi (ADNKS)
/// </summary>
public class TuikPopulationService : IPopulationService
{
    private static readonly Lazy<List<PopulationData>> _data = new(
        () => EmbeddedJsonDataProvider.Load<PopulationData>("population.json"));

    private static List<PopulationData> Data => _data.Value;

    public Task<PopulationData?> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var result = Data.FirstOrDefault(p => p.Year == year && p.Province == "Türkiye");
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<PopulationData>> GetByYearRangeAsync(int startYear, int endYear, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PopulationData> result = Data
            .Where(p => p.Year >= startYear && p.Year <= endYear && p.Province == "Türkiye")
            .OrderByDescending(p => p.Year)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<PopulationData>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PopulationData> result = Data
            .Where(p => string.Equals(p.Province, province, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(p => p.Year)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }
}
