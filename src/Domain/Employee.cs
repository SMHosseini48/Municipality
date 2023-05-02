using Domain.Enums;

namespace Domain;

public class Employee :BaseUser
{
    public string Name { get; set; }
    
    public string? ContrantorId { get; set; }
    public Contractor Contractor { get; set; }
    public override Role Role { get; set; } = Role.Employee;
    public IList<Cargo> Cargoes { get; set; }
}