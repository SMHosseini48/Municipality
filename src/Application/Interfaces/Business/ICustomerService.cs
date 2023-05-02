namespace Application.Interfaces.Business;

public interface ICustomerService
{
    public Task<object> GetCustomer(string email);
}