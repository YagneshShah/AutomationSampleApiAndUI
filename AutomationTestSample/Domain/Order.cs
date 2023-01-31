using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationTestSample.Domain;

[Table("Orders")] //none of the attributes are enforced in this database table
public class Order
{
    [Key]
    public int Id { get; init; }

    [Required, MaxLength(16)]
    public string AccessionNumber { get; set; }
    
    [Required, MaxLength(5)]
    public string OrgCode { get; set; }
    
    [Required]
    public int SiteId { get; set; }
    
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    [Required, MaxLength(5)] 
    public string Modality { get; set; }

    [Required] 
    public DateTimeOffset StudyDateTime { get; set; }

    [Required, MaxLength(5)]
    public Status Status { get; set; } = Status.SC;

    [Required] 
    public DateTimeOffset CreatedAt { get; init; }

    [Required]
    public DateTimeOffset UpdatedAt { get; set; }

    [Required]
    private readonly Patient? _patient;
    public Patient Patient
    {
        get => _patient ?? throw new InvalidOperationException("Patient not loaded.");
        init => _patient = value;
    }

    public Order(int patientId, string accessionNumber, string orgCode, int siteId, string modality, DateTimeOffset studyDateTime, Status status)
    {
        PatientId = patientId;
        AccessionNumber = accessionNumber;
        OrgCode = orgCode;
        SiteId = siteId;
        Modality = modality;
        StudyDateTime = studyDateTime;
        Status = status;
    }
}
