using TuikMcp.Domain.Entities;

namespace TuikMcp.Domain.Interfaces;

/// <summary>
/// TÜİK eğitim verileri servisi interface'i
/// </summary>
public interface IEducationService
{
    /// <summary>
    /// Belirtilen ilin eğitim verilerini getirir
    /// </summary>
    Task<IReadOnlyList<EducationData>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıla ait tüm illerin eğitim verilerini getirir
    /// </summary>
    Task<IReadOnlyList<EducationData>> GetByYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen yıl ve ilin eğitim verisini getirir
    /// </summary>
    Task<EducationData?> GetByProvinceAndYearAsync(string province, int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Eğitim seviyesi sıralaması getirir (üniversite mezun oranına göre)
    /// </summary>
    Task<IReadOnlyList<EducationData>> GetRankingByUniversityRateAsync(int year, CancellationToken cancellationToken = default);
}
