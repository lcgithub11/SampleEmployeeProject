using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<FamilyMember> FamilyMembers { get; set; }

}
