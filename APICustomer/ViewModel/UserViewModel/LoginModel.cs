using System.ComponentModel.DataAnnotations;

namespace APICustomer.ViewModel.UserViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
}
