using System.Text.Json.Serialization;
using Domain.Enums;
using Newtonsoft.Json.Converters;

namespace Application.Dtos;

public class CargoItemDto
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ResidueType Type { get; set; }
    public int Unit { get; set; }
}