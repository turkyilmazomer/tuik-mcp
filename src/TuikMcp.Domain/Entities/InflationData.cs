namespace TuikMcp.Domain.Entities;

/// <summary>
/// Enflasyon verilerini temsil eden entity (TÜFE - Tüketici Fiyat Endeksi)
/// </summary>
public class InflationData
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string Period { get; set; } = string.Empty;
    public double MonthlyChange { get; set; }
    public double AnnualChange { get; set; }
    public double TwelveMonthAverage { get; set; }
    public double ConsumerPriceIndex { get; set; }
}
