using TuikMcp.Application.DTOs;
using TuikMcp.Domain.Interfaces;

namespace TuikMcp.Application.Services;

/// <summary>
/// Enflasyon verilerini yöneten uygulama servisi
/// </summary>
public class InflationAppService
{
    private readonly IInflationService _inflationService;

    public InflationAppService(IInflationService inflationService)
    {
        _inflationService = inflationService;
    }

    public async Task<InflationDto?> GetInflationByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var data = await _inflationService.GetByPeriodAsync(year, month, cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    public async Task<IReadOnlyList<InflationDto>> GetInflationByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var dataList = await _inflationService.GetByYearAsync(year, cancellationToken);
        return dataList.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<InflationDto?> GetLatestInflationAsync(CancellationToken cancellationToken = default)
    {
        var data = await _inflationService.GetLatestAsync(cancellationToken);
        return data is null ? null : MapToDto(data);
    }

    private static InflationDto MapToDto(Domain.Entities.InflationData data) =>
        new(
            Year: data.Year,
            Month: data.Month,
            Period: data.Period,
            MonthlyChange: data.MonthlyChange,
            AnnualChange: data.AnnualChange,
            TwelveMonthAverage: data.TwelveMonthAverage,
            ConsumerPriceIndex: data.ConsumerPriceIndex
        );
}
