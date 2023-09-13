public class Employee : BaseEntity
{
    public string RegisterNumber { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthdate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<FamilyMember> FamilyMembers { get; set; }
}
