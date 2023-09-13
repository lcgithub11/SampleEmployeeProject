public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string UpdatedBy { get; set; }
}
