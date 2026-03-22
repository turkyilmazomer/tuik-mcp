using TuikMcp.Domain.Entities;

namespace TuikMcp.Domain.Interfaces;

/// <summary>
/// TÜİK enflasyon verileri servisi interface'i
/// </summary>
public interface IInflationService
{
    /// <summary>
    /// Belirtilen yıl ve aya ait enflasyon verilerini getirir
    /// </summary>
    Task<InflationData?> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıla ait tüm enflasyon verilerini getirir
    /// </summary>
    Task<IReadOnlyList<InflationData>> GetByYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// En güncel enflasyon verilerini getirir
    /// </summary>
    Task<InflationData?> GetLatestAsync(CancellationToken cancellationToken = default);
}
