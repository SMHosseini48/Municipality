using Application.Dtos;

namespace Application.Interfaces.Business;

public interface IUserService
{
    // public Task<object> LoginEmployee(UserLoginDto userLoginDto);
    // public Task<object> LoginCustomer(UserLoginDto userLoginDto);
    // public Task<object> LoginContractor(UserLoginDto userLoginDto);
    // public Task<object> LoginSupervisor(UserLoginDto userLoginDto);
    // public Task<object> LoginMunicipality(UserLoginDto userLoginDto);
    public Task<object> Login(UserLoginDto userLoginDto);
    public  Task<object> RegisterEmployee(RegisterEmployeeDto registerEmployeeDto);
    public  Task<object> RegisterContractor(RegisterContractorDto registerContractorDto);
    public  Task<object> RegisterSupervisor(RegisterSupervisorDto registerSupervisorDto);
    public  Task<object> RegisterMunicipality(RegisterMunicipalityDto registerMunicipalityDto);
    public  Task<object> RegisterCustomer(RegisterCustomerDto registerCustomerDto);
}