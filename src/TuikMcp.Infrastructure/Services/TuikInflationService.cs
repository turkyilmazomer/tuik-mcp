using TuikMcp.Domain.Entities;
using TuikMcp.Domain.Interfaces;
using TuikMcp.Infrastructure.Data;

namespace TuikMcp.Infrastructure.Services;

/// <summary>
/// Gömülü JSON verilerinden enflasyon (TÜFE) istatistiklerini sunan servis.
/// Veri kaynağı: TÜİK Tüketici Fiyat Endeksi
/// </summary>
public class TuikInflationService : IInflationService
{
    private static readonly Lazy<List<InflationData>> _data = new(
        () => EmbeddedJsonDataProvider.Load<InflationData>("inflation.json"));

    private static List<InflationData> Data => _data.Value;

    public Task<InflationData?> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var result = Data.FirstOrDefault(i => i.Year == year && i.Month == month);
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<InflationData>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<InflationData> result = Data
            .Where(i => i.Year == year)
            .OrderByDescending(i => i.Month)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<InflationData?> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var result = Data
            .OrderByDescending(i => i.Year)
            .ThenByDescending(i => i.Month)
            .FirstOrDefault();

        return Task.FromResult(result);
    }
}
