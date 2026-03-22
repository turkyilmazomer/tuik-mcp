namespace TuikMcp.Application.DTOs;

/// <summary>
/// İl bazında işsizlik verisi DTO
/// </summary>
public record ProvinceUnemploymentDto(
    int Year,
    string Province,
    double UnemploymentRate,
    double YouthUnemploymentRate,
    double LabourForceParticipationRate,
    double EmploymentRate,
    long LabourForce,
    long NumberOfUnemployed
);
