using APICustomer.Models;
using APICustomer.ViewModel.CustomerViewModel;

namespace APICustomer.Repositories.IRepo
{
    public interface ICustomerRepo
    {
        Task<List<Customer>> GetListCustomers(FilterCustomer filter);
        Task<string> Create(CreateCustomer model);
        Task<int> Update(Guid Id, UpdateCustomer model);
        Task<int> Delete(Guid Id);
        Task<Customer> GetById(Guid Id);
        Task<Customer> GetByPhoneNumber(string phonenumber);
    }
}
