using System.IdentityModel.Tokens.Jwt;
using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces.Business;
using Application.Interfaces.Data;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class CargoService : ICargoService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Cargo> _cargoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<BaseUser> _userManager;

    public CargoService(IMapper mapper, IGenericRepository<Cargo> cargoRepository,
        IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager)
    {
        _mapper = mapper;
        _cargoRepository = cargoRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<object> RegisterCargo(RegisterCargoDto registerCargoDto)
    {
        var customer = await _userManager.FindByIdAsync(registerCargoDto.CustomerId);
        if (customer == null) throw new NotFoundException("Customer does not exist");


        var email = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email)
            .Value;
        
        var employee = await _userManager.FindByEmailAsync(email);
        var cargo = _mapper.Map<Cargo>(registerCargoDto);
        cargo.EmployeeId =  employee.Id;
        await _cargoRepository.InsertAsync(cargo);
        await _cargoRepository.SaveChanges();
        return cargo.Id;
    }

    public async Task<object> ReviewCargo(ReviewCargoDto reviewCargoDto)
    {
        var email = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email)
            .Value;
        var user = await _userManager.FindByEmailAsync(email);

        var cargo = await _cargoRepository.GetOneByQueryAsync(x => x.Id == reviewCargoDto.Id);
        cargo.ReviewedPrice = reviewCargoDto.ReviewedPoints;
        cargo.SupervisorId =  user.Id;
        _cargoRepository.Update(cargo);
        await _cargoRepository.SaveChanges();

        return cargo.ReviewedPrice;
    }
}