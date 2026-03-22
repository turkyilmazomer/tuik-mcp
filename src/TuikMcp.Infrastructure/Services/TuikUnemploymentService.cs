using TuikMcp.Domain.Entities;
using TuikMcp.Domain.Interfaces;
using TuikMcp.Infrastructure.Data;

namespace TuikMcp.Infrastructure.Services;

/// <summary>
/// Gömülü JSON verilerinden işsizlik istatistiklerini sunan servis.
/// Veri kaynağı: TÜİK Hanehalkı İşgücü Araştırması
/// </summary>
public class TuikUnemploymentService : IUnemploymentService
{
    private static readonly Lazy<List<UnemploymentData>> _data = new(
        () => EmbeddedJsonDataProvider.Load<UnemploymentData>("unemployment.json"));

    private static List<UnemploymentData> Data => _data.Value;

    public Task<UnemploymentData?> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var result = Data.FirstOrDefault(u => u.Year == year && u.Month == month);
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<UnemploymentData>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<UnemploymentData> result = Data
            .Where(u => u.Year == year)
            .OrderByDescending(u => u.Month)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(result);
    }

    public Task<UnemploymentData?> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var result = Data
            .OrderByDescending(u => u.Year)
            .ThenByDescending(u => u.Month)
            .FirstOrDefault();

        return Task.FromResult(result);
    }
}
