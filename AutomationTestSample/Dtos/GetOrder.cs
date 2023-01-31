namespace AutomationTestSample.Dtos;

public record GetOrder(
  int Id,
  string AccessionNumber,
  string OrgCode,
  string SiteName,
  string PatientMrn,
  string PatientName,
  string Modality,
  DateTimeOffset StudyDateTime,
  string Status,
  DateTimeOffset CreatedAt,
  DateTimeOffset UpdatedAt
);
