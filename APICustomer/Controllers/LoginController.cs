using APICustomer.Repositories.IRepo;
using APICustomer.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICustomer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public LoginController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _userRepo.Login(model);
            if (response == null) return BadRequest("Sai tài khoản hoặc mật khẩu");
            return Ok(response);
        }
    }
}
