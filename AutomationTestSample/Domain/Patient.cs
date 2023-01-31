using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationTestSample.Domain;

[Table("Patients")] //none of the attributes are enforced in this database table
public class Patient
{
    [Key]
    public int Id { get; set; }

    // This field has intentionally bigger max lengths in higher levels in the stack, eg PostOrder, but this DB length is not properly enforced and so will allow bigger lengths
    [Required, MaxLength(12)]
    public string Mrn { get; set; }

    [Required, MaxLength(64)]
    public string FirstName { get; set; }

    [Required, MaxLength(64)]
    public string LastName { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public Patient(string mrn, string firstName, string lastName)
    {
        Mrn = mrn;
        FirstName = firstName;
        LastName = lastName;
    }

}
