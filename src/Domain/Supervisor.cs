using Domain.Enums;

namespace Domain;

public class Supervisor : BaseUser
{
    public string Name { get; set; }

    public override Role Role { get; set; } = Role.Supervisor;
    public IList<Cargo> Cargoes { get; set; }

    public string? MunicipalityId { get; set; }
    public Municipality Municipality { get; set; }
}