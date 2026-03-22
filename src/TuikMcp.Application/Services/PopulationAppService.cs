using TuikMcp.Application.DTOs;
using TuikMcp.Domain.Interfaces;

namespace TuikMcp.Application.Services;

/// <summary>
/// Nüfus verilerini yöneten uygulama servisi
/// </summary>
public class PopulationAppService
{
    private readonly IPopulationService _populationService;

    public PopulationAppService(IPopulationService populationService)
    {
        _populationService = populationService;
    }

    public async Task<PopulationDto?> GetPopulationByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var data = await _populationService.GetByYearAsync(year, cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    public async Task<IReadOnlyList<PopulationDto>> GetPopulationByYearRangeAsync(int startYear, int endYear, CancellationToken cancellationToken = default)
    {
        var dataList = await _populationService.GetByYearRangeAsync(startYear, endYear, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<PopulationDto>> GetPopulationByProvinceAsync(string province, CancellationToken cancellationToken = default)
    {
        var dataList = await _populationService.GetByProvinceAsync(province, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    private static PopulationDto MapToDto(Domain.Entities.PopulationData data) =>
        new(
            Year: data.Year,
            TotalPopulation: data.TotalPopulation,
            MalePopulation: data.MalePopulation,
            FemalePopulation: data.FemalePopulation,
            AnnualGrowthRate: data.AnnualGrowthRate,
            Province: data.Province
        );
}
