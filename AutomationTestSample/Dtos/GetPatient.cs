namespace AutomationTestSample.Dtos;

public record GetPatient(
  int Id,
  string Mrn,
  string FirstName,
  string LastName,
  DateTimeOffset CreatedAt,
  DateTimeOffset UpdatedAt
);
