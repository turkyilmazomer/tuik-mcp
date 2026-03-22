namespace TuikMcp.Domain.Entities;

/// <summary>
/// İl bazında işsizlik verilerini temsil eden entity (TÜİK Bölgesel İşgücü İstatistikleri)
/// </summary>
public class ProvinceUnemploymentData
{
    public int Year { get; set; }
    public string Province { get; set; } = string.Empty;
    public double UnemploymentRate { get; set; }
    public double YouthUnemploymentRate { get; set; }
    public double LabourForceParticipationRate { get; set; }
    public double EmploymentRate { get; set; }
    public long LabourForce { get; set; }
    public long NumberOfUnemployed { get; set; }
}
