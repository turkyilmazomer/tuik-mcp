namespace TuikMcp.Domain.Entities;

/// <summary>
/// İşsizlik verilerini temsil eden entity
/// </summary>
public class UnemploymentData
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string Period { get; set; } = string.Empty;
    public double UnemploymentRate { get; set; }
    public double YouthUnemploymentRate { get; set; }
    public double LabourForceParticipationRate { get; set; }
    public double EmploymentRate { get; set; }
    public long NumberOfUnemployed { get; set; }
}
