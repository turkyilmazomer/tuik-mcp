namespace TuikMcp.Domain.Entities;

/// <summary>
/// Eğitim verilerini temsil eden entity (TÜİK Eğitim İstatistikleri)
/// İl bazında eğitim seviyesi dağılımı
/// </summary>
public class EducationData
{
    public int Year { get; set; }
    public string Province { get; set; } = string.Empty;
    public long TotalPopulationOver15 { get; set; }
    public double LiteracyRate { get; set; }
    public double HighSchoolRate { get; set; }
    public double UniversityRate { get; set; }
    public long UniversityGraduates { get; set; }
    public long HighSchoolGraduates { get; set; }
    public long PrimarySchoolGraduates { get; set; }
    public long IlliteratePopulation { get; set; }
    public double AverageSchoolingYears { get; set; }
}
