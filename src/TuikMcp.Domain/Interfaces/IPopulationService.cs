using TuikMcp.Domain.Entities;

namespace TuikMcp.Domain.Interfaces;

/// <summary>
/// TÜİK nüfus verileri servisi interface'i
/// </summary>
public interface IPopulationService
{
    /// <summary>
    /// Belirtilen yıla ait nüfus verilerini getirir
    /// </summary>
    Task<PopulationData?> GetByYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıl aralığındaki nüfus verilerini getirir
    /// </summary>
    Task<IReadOnlyList<PopulationData>> GetByYearRangeAsync(int startYear, int endYear, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ile ait nüfus verilerini getirir
    /// </summary>
    Task<IReadOnlyList<PopulationData>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default);
}
