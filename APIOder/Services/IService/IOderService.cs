using APIOder.ViewModel.CustomerViewModel;
using APIOder.ViewModel.OderViewModel;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace APIOder.Services.IService
{
    public interface IOderService
    {
        [Get("/api/Customers/GetByPhoneNumber/{phonenumber}")]
        Task<string> FindCustomerByPhoneNumber(string phonenumber);
        [Post("/api/Customers/CreateCustomer")]
        Task<string> AddCustomer(CreateCustomer create);
        [Get("/api/Customers/GetById/{Id}")]
        Task<string> GetCustomer(string Id);

    }
}
