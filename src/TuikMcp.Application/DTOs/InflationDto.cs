namespace TuikMcp.Application.DTOs;

/// <summary>
/// Enflasyon verisi DTO
/// </summary>
public record InflationDto(
    int Year,
    int Month,
    string Period,
    double MonthlyChange,
    double AnnualChange,
    double TwelveMonthAverage,
    double ConsumerPriceIndex
);
