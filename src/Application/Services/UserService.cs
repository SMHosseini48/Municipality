using Application.Dtos;
using Application.Interfaces.Business;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly UserManager<BaseUser> _userManager;


    public UserService(IMapper mapper, IJwtService jwtService, UserManager<BaseUser> userManager)
    {
        _mapper = mapper;
        _jwtService = jwtService;
        _userManager = userManager;
    }

    public async Task<object> RegisterMunicipality(RegisterMunicipalityDto registerMunicipalityDto)
    {
        var user = await _userManager.FindByEmailAsync(registerMunicipalityDto.Email);
        if (user != null) return new Exception("user already exist");

        var municipality = _mapper.Map<Municipality>(registerMunicipalityDto);
        municipality.UserName = municipality.Email;
        await _userManager.CreateAsync(municipality, registerMunicipalityDto.Password);
        return await _jwtService.CreateToken(municipality);
    }

    public async Task<object> RegisterSupervisor(RegisterSupervisorDto registerSupervisorDto)
    {
        var user = await _userManager.FindByEmailAsync(registerSupervisorDto.Email);
        if (user != null) return new Exception("user already exist");

        var supervisor = _mapper.Map<Supervisor>(registerSupervisorDto);
        supervisor.UserName = supervisor.Email;
        await _userManager.CreateAsync(supervisor, registerSupervisorDto.Password);
        return await _jwtService.CreateToken(supervisor);
    }

    public async Task<object> RegisterContractor(RegisterContractorDto registerContractorDto)
    {
        var user = await _userManager.FindByEmailAsync(registerContractorDto.Email);
        if (user != null) return new Exception("user already exist");

        var contractor = _mapper.Map<Contractor>(registerContractorDto);
        contractor.UserName = contractor.Email;
        await _userManager.CreateAsync(contractor, registerContractorDto.Password);
        return await _jwtService.CreateToken(contractor);
    }

    public async Task<object> RegisterEmployee(RegisterEmployeeDto registerEmployeeDto)
    {
        var user = await _userManager.FindByEmailAsync(registerEmployeeDto.Email);
        if (user != null) return new Exception("user already exist");

        var employee = _mapper.Map<Employee>(registerEmployeeDto);
        employee.UserName = employee.Email;
        await _userManager.CreateAsync(employee, registerEmployeeDto.Password);
        return await _jwtService.CreateToken(employee);
    }

    public async Task<object> RegisterCustomer(RegisterCustomerDto registerCustomerDto)
    {
        var user = await _userManager.FindByEmailAsync(registerCustomerDto.Email);
        if (user != null) return new Exception("user already exist");

        var customer = _mapper.Map<Customer>(registerCustomerDto);
        customer.UserName = customer.Email;
        await _userManager.CreateAsync(customer, registerCustomerDto.Password);
        return await _jwtService.CreateToken(customer);
    }


    public async Task<object> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if (user == null) return new Exception("user does not exsit");

        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
        if (!userHasValidPassword) return new Exception("wrong email/password combination");

        return await _jwtService.CreateToken(user);
    }

    // public async Task<object> LoginSupervisor(UserLoginDto userLoginDto)
    // {
    //     var user = await _supervisorManager.FindByEmailAsync(userLoginDto.Email);
    //     if (user == null) return new Exception("user does not exsit");
    //
    //     var userHasValidPassword = await _supervisorManager.CheckPasswordAsync(user, userLoginDto.Password);
    //     if (!userHasValidPassword) return new Exception("wrong email/password combination");
    //
    //     return await _jwtService.CreateToken(user);
    // }
    //
    // public async Task<object> LoginContractor(UserLoginDto userLoginDto)
    // {
    //     var user = await _contractorManager.FindByEmailAsync(userLoginDto.Email);
    //     if (user == null) return new Exception("user does not exsit");
    //
    //     var userHasValidPassword = await _contractorManager.CheckPasswordAsync(user, userLoginDto.Password);
    //     if (!userHasValidPassword) return new Exception("wrong email/password combination");
    //
    //     return await _jwtService.CreateToken(user);
    // }
    //
    // public async Task<object> LoginCustomer(UserLoginDto userLoginDto)
    // {
    //     var user = await _customerManager.FindByEmailAsync(userLoginDto.Email);
    //     if (user == null) return new Exception("user does not exsit");
    //
    //     var userHasValidPassword = await _customerManager.CheckPasswordAsync(user, userLoginDto.Password);
    //     if (!userHasValidPassword) return new Exception("wrong email/password combination");
    //
    //     return await _jwtService.CreateToken(user);
    // }
    //
    // public async Task<object> LoginEmployee(UserLoginDto userLoginDto)
    // {
    //     var user = await _employeeManager.FindByEmailAsync(userLoginDto.Email);
    //     if (user == null) return new Exception("user does not exsit");
    //
    //     var userHasValidPassword = await _employeeManager.CheckPasswordAsync(user, userLoginDto.Password);
    //     if (!userHasValidPassword) return new Exception("wrong email/password combination");
    //
    //     return await _jwtService.CreateToken(user);
    // }
}