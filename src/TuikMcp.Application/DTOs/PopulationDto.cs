namespace TuikMcp.Application.DTOs;

/// <summary>
/// Nüfus verisi DTO
/// </summary>
public record PopulationDto(
    int Year,
    long TotalPopulation,
    long MalePopulation,
    long FemalePopulation,
    double AnnualGrowthRate,
    string? Province
);
