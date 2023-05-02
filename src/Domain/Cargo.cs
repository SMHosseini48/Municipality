namespace Domain;

public class Cargo
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int TotalPoints => CargoItems.Sum(x => x.Point);
    
    public int? ReviewedPrice { get; set; }
    public IList<CargoItem> CargoItems { get; set; }

    public string? EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public string? CustomerId { get; set; }
    public Customer Customer { get; set; }

    public string? SupervisorId { get; set; }
    public Supervisor Supervisor { get; set; }
}