using TuikMcp.Domain.Entities;

namespace TuikMcp.Domain.Interfaces;

/// <summary>
/// TÜİK il bazında işsizlik verileri servisi interface'i
/// </summary>
public interface IProvinceUnemploymentService
{
    /// <summary>
    /// Belirtilen ilin işsizlik verilerini getirir (yıllara göre)
    /// </summary>
    Task<IReadOnlyList<ProvinceUnemploymentData>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıla ait tüm illerin işsizlik verilerini getirir
    /// </summary>
    Task<IReadOnlyList<ProvinceUnemploymentData>> GetByYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yılda işsizlik oranına göre sıralama getirir
    /// </summary>
    Task<IReadOnlyList<ProvinceUnemploymentData>> GetRankingByUnemploymentRateAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıl ve ilin işsizlik verisini getirir
    /// </summary>
    Task<ProvinceUnemploymentData?> GetByProvinceAndYearAsync(string province, int year, CancellationToken cancellationToken = default);
}
