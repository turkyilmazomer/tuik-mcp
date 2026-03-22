namespace TuikMcp.Application.DTOs;

/// <summary>
/// Eğitim verisi DTO
/// </summary>
public record EducationDto(
    int Year,
    string Province,
    long TotalPopulationOver15,
    double LiteracyRate,
    double HighSchoolRate,
    double UniversityRate,
    long UniversityGraduates,
    long HighSchoolGraduates,
    long PrimarySchoolGraduates,
    long IlliteratePopulation,
    double AverageSchoolingYears
);
