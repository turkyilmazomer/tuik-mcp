using TuikMcp.Application.DTOs;
using TuikMcp.Domain.Interfaces;

namespace TuikMcp.Application.Services;

/// <summary>
/// İşsizlik verilerini yöneten uygulama servisi
/// </summary>
public class UnemploymentAppService
{
    private readonly IUnemploymentService _unemploymentService;

    public UnemploymentAppService(IUnemploymentService unemploymentService)
    {
        _unemploymentService = unemploymentService;
    }

    public async Task<UnemploymentDto?> GetUnemploymentByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var data = await _unemploymentService.GetByPeriodAsync(year, month, cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    public async Task<IReadOnlyList<UnemploymentDto>> GetUnemploymentByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var dataList = await _unemploymentService.GetByYearAsync(year, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<UnemploymentDto?> GetLatestUnemploymentAsync(CancellationToken cancellationToken = default)
    {
        var data = await _unemploymentService.GetLatestAsync(cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    private static UnemploymentDto MapToDto(Domain.Entities.UnemploymentData data) =>
        new(
            Year: data.Year,
            Month: data.Month,
            Period: data.Period,
            UnemploymentRate: data.UnemploymentRate,
            YouthUnemploymentRate: data.YouthUnemploymentRate,
            LabourForceParticipationRate: data.LabourForceParticipationRate,
            EmploymentRate: data.EmploymentRate,
            NumberOfUnemployed: data.NumberOfUnemployed
        );
}
