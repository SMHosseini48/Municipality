using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces.Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }


    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Policy = "Customer")]
    public async Task<IActionResult> GetCustomer()
    {
        var userEmail = HttpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email).Value;
        var result = await _customerService.GetCustomer(userEmail);
        return Ok(result);
    }
}