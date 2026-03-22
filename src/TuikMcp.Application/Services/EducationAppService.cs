using TuikMcp.Application.DTOs;
using TuikMcp.Domain.Interfaces;

namespace TuikMcp.Application.Services;

/// <summary>
/// Eğitim verilerini yöneten uygulama servisi
/// </summary>
public class EducationAppService
{
    private readonly IEducationService _educationService;

    public EducationAppService(IEducationService educationService)
    {
        _educationService = educationService;
    }

    public async Task<IReadOnlyList<EducationDto>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default)
    {
        var dataList = await _educationService.GetByProvinceAsync(province, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<EducationDto>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var dataList = await _educationService.GetByYearAsync(year, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<EducationDto?> GetByProvinceAndYearAsync(string province, int year, CancellationToken cancellationToken = default)
    {
        var data = await _educationService.GetByProvinceAndYearAsync(province, year, cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    public async Task<IReadOnlyList<EducationDto>> GetRankingByUniversityRateAsync(int year, CancellationToken cancellationToken = default)
    {
        var dataList = await _educationService.GetRankingByUniversityRateAsync(year, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    private static EducationDto MapToDto(Domain.Entities.EducationData data) =>
        new(
            Year: data.Year,
            Province: data.Province,
            TotalPopulationOver15: data.TotalPopulationOver15,
            LiteracyRate: data.LiteracyRate,
            HighSchoolRate: data.HighSchoolRate,
            UniversityRate: data.UniversityRate,
            UniversityGraduates: data.UniversityGraduates,
            HighSchoolGraduates: data.HighSchoolGraduates,
            PrimarySchoolGraduates: data.PrimarySchoolGraduates,
            IlliteratePopulation: data.IlliteratePopulation,
            AverageSchoolingYears: data.AverageSchoolingYears
        );
}
