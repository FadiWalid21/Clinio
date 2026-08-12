namespace Clinio.Domain.Entities.Clinics;

public class ClinicImage
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public bool IsCover { get; set; }
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;
}