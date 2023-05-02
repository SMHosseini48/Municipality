
namespace Application.Dtos;

public class CustomerViewDto
{
    public string Name { get; set; }
    public long NationalCode { get; set; }
    public int TotalPoints { get; set; }
    public IList<CargoViewDto> Cargoes { get; set; }
}