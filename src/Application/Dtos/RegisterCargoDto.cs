namespace Application.Dtos;

public class RegisterCargoDto
{
    public string Name { get; set; }
    public IList<CargoItemDto> CargoItems { get; set; }

    public string CustomerId { get; set; }
}