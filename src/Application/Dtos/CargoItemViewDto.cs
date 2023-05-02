
using Domain.Enums;

namespace Application.Dtos;

public class CargoItemViewDto
{
    public ResidueType Type { get; set; }
    public int Unit { get; set; }
    public int Point { get; set; }
}