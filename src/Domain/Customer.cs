using Domain.Enums;

namespace Domain;

public class Customer : BaseUser
{
    public string Name { get; set; }
    public long NationalCode { get; set; }
    public override Role Role { get; set; } = Role.Customer;
    public int TotalPoints => Cargoes.Sum(x => x.TotalPoints);
    public IList<Cargo> Cargoes { get; set; }
}