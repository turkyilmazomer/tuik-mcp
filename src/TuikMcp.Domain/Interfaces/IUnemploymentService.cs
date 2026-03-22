using TuikMcp.Domain.Entities;

namespace TuikMcp.Domain.Interfaces;

/// <summary>
/// TÜİK işsizlik verileri servisi interface'i
/// </summary>
public interface IUnemploymentService
{
    /// <summary>
    /// Belirtilen yıl ve aya ait işsizlik verilerini getirir
    /// </summary>
    Task<UnemploymentData?> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıla ait tüm işsizlik verilerini getirir
    /// </summary>
    Task<IReadOnlyList<UnemploymentData>> GetByYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// En güncel işsizlik verilerini getirir
    /// </summary>
    Task<UnemploymentData?> GetLatestAsync(CancellationToken cancellationToken = default);
}
