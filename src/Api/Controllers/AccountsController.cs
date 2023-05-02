using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces.Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AccountsController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Policy = "Municipality")]
    [Route("register/municipality")]
    [HttpPost]
    public async Task<IActionResult> RegisterMunicipality([FromBody] RegisterMunicipalityDto registerMunicipalityDto)
    {
        if (registerMunicipalityDto.Password != registerMunicipalityDto.ConfirmPassword)
            throw new UnAuthorizedException("confirm pass word does not match");
        var result = await _userService.RegisterMunicipality(registerMunicipalityDto);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Policy = "Municipality")]
    [Route("register/supervisor")]
    [HttpPost]
    public async Task<IActionResult> RegisterSupervisor([FromBody] RegisterSupervisorDto registerSupervisorDto)
    {
        if (registerSupervisorDto.Password != registerSupervisorDto.ConfirmPassword)
            throw new UnAuthorizedException("confirm pass word does not match");

        var result = await _userService.RegisterSupervisor(registerSupervisorDto);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Policy = "MunicipalityOrContractor")]
    [Route("register/employee")]
    [HttpPost]
    public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeDto registerEmployeeDto)
    {
        if (registerEmployeeDto.Password != registerEmployeeDto.ConfirmPassword)
            throw new UnAuthorizedException("confirm pass word does not match");

        var result = await _userService.RegisterEmployee(registerEmployeeDto);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Policy = "Municipality")]
    [Route("register/contractor")]
    [HttpPost]
    public async Task<IActionResult> RegisterContractor([FromBody] RegisterContractorDto registerContractorDto)
    {
        if (registerContractorDto.Password != registerContractorDto.ConfirmPassword)
            throw new UnAuthorizedException("confirm pass word does not match");

        var result = await _userService.RegisterContractor(registerContractorDto);
        return Ok(result);
    }

    [Route("register/customer")]
    [HttpPost]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDto registerCustomerDto)
    {
        if (registerCustomerDto.Password != registerCustomerDto.ConfirmPassword)
            throw new UnAuthorizedException("confirm pass word does not match");

        var result = await _userService.RegisterCustomer(registerCustomerDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var result = await _userService.Login(userLoginDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        var result = await _jwtService.RefreshToken(refreshRequest);
        return Ok(result);
    }
}