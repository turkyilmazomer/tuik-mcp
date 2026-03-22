namespace TuikMcp.Domain.Entities;

/// <summary>
/// Nüfus verilerini temsil eden entity
/// </summary>
public class PopulationData
{
    public int Year { get; set; }
    public long TotalPopulation { get; set; }
    public long MalePopulation { get; set; }
    public long FemalePopulation { get; set; }
    public double AnnualGrowthRate { get; set; }
    public string? Province { get; set; }
}
