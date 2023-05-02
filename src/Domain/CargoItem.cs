using Domain.Enums;

namespace Domain;

public class CargoItem
{
    public int Id { get; set; }
    public ResidueType Type { get; set; }
    public int Unit { get; set; }

    public int? CargoId { get; set; }
    public Cargo Cargo { get; set; }

    public int Point => (int) Type;
    
    // public static readonly dynamic[,] TypeUnitCost =
    // {
    //     {1, "Iron", 5},
    //     {2, "Plastic", 3},
    //     {3, "Bread", 2}
    // };
}