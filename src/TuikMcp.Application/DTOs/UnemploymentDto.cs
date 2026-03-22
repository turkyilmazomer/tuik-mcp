namespace TuikMcp.Application.DTOs;

/// <summary>
/// İşsizlik verisi DTO
/// </summary>
public record UnemploymentDto(
    int Year,
    int Month,
    string Period,
    double UnemploymentRate,
    double YouthUnemploymentRate,
    double LabourForceParticipationRate,
    double EmploymentRate,
    long NumberOfUnemployed
);
