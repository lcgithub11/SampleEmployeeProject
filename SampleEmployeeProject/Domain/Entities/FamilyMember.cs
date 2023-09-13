using System.ComponentModel.DataAnnotations;

public class FamilyMember : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Surname { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Birthdate { get; set; }

    [Required]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public FamilyType Type { get; set; } // Enum: Spouse, Child
}

public enum FamilyType
{
    Spouse,
    Child
}
