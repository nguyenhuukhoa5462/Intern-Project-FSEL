using APICustomer.ViewModel.UserViewModel;

namespace APICustomer.Repositories.IRepo
{
    public interface IUserRepo
    {
        public Task<string> Login(LoginModel model);
    }
}
