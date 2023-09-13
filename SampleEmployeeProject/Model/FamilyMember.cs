public class FamilyMember : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthdate { get; set; }
    public string PhoneNumber { get; set; }
    public Guid EmployeeId { get; set; }
    public FamilyType Type { get; set; } // Enum: Spouse, Child
}

public enum FamilyType
{
    Spouse,
    Child
}