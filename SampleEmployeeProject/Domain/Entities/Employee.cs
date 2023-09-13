using System.ComponentModel.DataAnnotations;

public class Employee : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string RegisterNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Surname { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DateOfBirth]
    public DateTime Birthdate { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; }

    [Required]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    // Relationship to FamilyMember
    public List<FamilyMember> FamilyMembers { get; set; }
}
