using Application.Dtos;
using Application.Interfaces.Business;
using Application.Interfaces.Data;
using AutoMapper;
using Domain;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(IGenericRepository<Customer> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<object> GetCustomer(string email)
    {
        var customer = await _customerRepository.GetOneByQueryAsync(x => x.Email == email ,includes:new List<string> { "Cargoes.CargoItems"});
        var customerView = _mapper.Map<CustomerViewDto>(customer);

        return customerView;
    }
}