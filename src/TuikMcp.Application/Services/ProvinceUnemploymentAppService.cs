using TuikMcp.Application.DTOs;
using TuikMcp.Domain.Interfaces;

namespace TuikMcp.Application.Services;

/// <summary>
/// İl bazında işsizlik verilerini yöneten uygulama servisi
/// </summary>
public class ProvinceUnemploymentAppService
{
    private readonly IProvinceUnemploymentService _provinceUnemploymentService;

    public ProvinceUnemploymentAppService(IProvinceUnemploymentService provinceUnemploymentService)
    {
        _provinceUnemploymentService = provinceUnemploymentService;
    }

    public async Task<IReadOnlyList<ProvinceUnemploymentDto>> GetByProvinceAsync(string province, CancellationToken cancellationToken = default)
    {
        var dataList = await _provinceUnemploymentService.GetByProvinceAsync(province, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<ProvinceUnemploymentDto>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var dataList = await _provinceUnemploymentService.GetByYearAsync(year, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<ProvinceUnemploymentDto>> GetRankingByUnemploymentRateAsync(int year, CancellationToken cancellationToken = default)
    {
        var dataList = await _provinceUnemploymentService.GetRankingByUnemploymentRateAsync(year, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<ProvinceUnemploymentDto?> GetByProvinceAndYearAsync(string province, int year, CancellationToken cancellationToken = default)
    {
        var data = await _provinceUnemploymentService.GetByProvinceAndYearAsync(province, year, cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    private static ProvinceUnemploymentDto MapToDto(Domain.Entities.ProvinceUnemploymentData data) =>
        new(
            Year: data.Year,
            Province: data.Province,
            UnemploymentRate: data.UnemploymentRate,
            YouthUnemploymentRate: data.YouthUnemploymentRate,
            LabourForceParticipationRate: data.LabourForceParticipationRate,
            EmploymentRate: data.EmploymentRate,
            LabourForce: data.LabourForce,
            NumberOfUnemployed: data.NumberOfUnemployed
        );
}
