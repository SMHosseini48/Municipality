namespace Application.Dtos;

public class CargoViewDto
{
    public string Name { get; set; }

    public int TotalPoints { get; set; }

    public int? ReviewedPrice { get; set; }
    public IList<CargoItemViewDto> CargoItems { get; set; }

}