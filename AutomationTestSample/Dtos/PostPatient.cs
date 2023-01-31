using System.ComponentModel.DataAnnotations;

namespace AutomationTestSample.Dtos;

public record PostPatient(
  [Required, MaxLength(12)]
  string Mrn,
  [Required, MaxLength(64)]
  string FirstName,
  [Required, MaxLength(64)]
  string LastName
);
