using Domain.Enums;

namespace Domain;

public class Municipality : BaseUser
{
    public string Name { get; set; }
    public override Role Role { get; set; } = Role.Municipality;
    public IList<Contractor> Contractors { get; set; }
    public IList<Supervisor> Supervisors { get; set; }
}