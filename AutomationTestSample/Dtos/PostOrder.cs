using System.ComponentModel.DataAnnotations;

namespace AutomationTestSample.Dtos;

public record PostOrder(
  [Required, MaxLength(15)] // this is wrong, the underlying DB has a max length of 12
  string PatientMrn,
  [Required, MaxLength(64)]
  string PatientFirstName,
  [Required, MaxLength(64)]
  string PatientLastName,
  
  [Required, MaxLength(16)]
  string AccessionNumber,
  [Required, MaxLength(5)]
  string OrgCode,
  [Required]
  int SiteId,
  [Required, MaxLength(5)]
  string Modality,
  [Required]
  DateTimeOffset StudyDateTime
);
