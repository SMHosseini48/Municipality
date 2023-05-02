using Application.Dtos;
using Domain;

namespace Application.Interfaces.Business;

public interface IJwtService
{
    Task<object> CreateToken(BaseUser baseUser);
    Task<object> RefreshToken(RefreshRequest refreshRequest);
}