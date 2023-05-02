using Domain.Enums;

namespace Domain;

public class Contractor : BaseUser
{
    public string Name { get; set; }
    public override Role Role { get; set; } = Role.Contractor;
    public IList<Employee> Employees { get; set; }

    public string? MunicipalityId { get; set; }
    public Municipality Municipality { get; set; }
}