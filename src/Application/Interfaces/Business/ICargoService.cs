using Application.Dtos;

namespace Application.Interfaces.Business;

public interface ICargoService
{
    public  Task<object> ReviewCargo(ReviewCargoDto reviewCargoDto);
    public  Task<object> RegisterCargo(RegisterCargoDto registerCargoDto);

}