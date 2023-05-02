using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces.Business;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CargosController : ControllerBase
{
    private readonly ICargoService _cargoService;

    public CargosController(ICargoService cargoService)
    {
        _cargoService = cargoService;
    }

    [HttpPost]
    [Route("register")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
    public async Task<IActionResult> CargoRegister([FromBody] RegisterCargoDto registerCargoDto)
    {
        var result = await _cargoService.RegisterCargo(registerCargoDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("review")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Supervisor")]
    public async Task<IActionResult> CargoReview([FromBody] ReviewCargoDto reviewCargoDto)
    {
        var result = await _cargoService.ReviewCargo(reviewCargoDto);
        return Ok(result);
    }
}