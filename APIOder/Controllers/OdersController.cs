using APIOder.Repositories.IRepo;
using APIOder.ViewModel.OderViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using APIOder.ViewModel.CustomerViewModel;
using Microsoft.AspNetCore.Authorization;
using APIOder.Services.Service;
using APIOder.Services.IService;
using APIOder.Models;
using Refit;
using System.Net.Http.Headers;


namespace APIOder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdersController : ControllerBase
    {
        private readonly IOderRepo _oderRepo;
        private readonly IOderService _oderService;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        public OdersController(IOderRepo oderRepo, HttpClient httpClient, IOderService oderService)
        {
            _oderRepo = oderRepo;
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _oderService = oderService;
        }

        [HttpGet]
        [Route("FindCustomer/{phonenumber}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> FindCustomer(string phonenumber)
        {
            var response = await _oderService.FindCustomerByPhoneNumber(phonenumber);
            if (response == "Không tìm thấy") return Ok(response);

            var customer = JsonConvert.DeserializeObject<ViewModelCustomer>(response);
            return Ok(customer);
        }

        [HttpPost]
        [Route("CreateOder")]
        public async Task<IActionResult> Create([FromBody] CreateOder create)
        {
            var result = await _oderRepo.Create(create);
            if (result == null) return BadRequest("Đã xảy ra lỗi, vui lòng thử lại");
            return Ok(result);

        }

        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomer create)
        {
            var response = await _oderService.AddCustomer(create);
            return Ok(response);

        }
        [HttpGet]
        [Route("GetByIdOder/{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var oder = await _oderRepo.GetByIdOder(Guid.Parse(Id));
            if (oder != null)
            {
                var response = await _oderService.GetCustomer(oder.OderObj.IdCustomer.ToString());

                if (response == "Không tìm thấy khách hàng")
                {
                    var responses = new
                    {
                        KhachHang = response,
                        HoaDon = oder.OderObj,
                    };

                    return Ok(responses);
                }
                else
                {
                    var customer = JsonConvert.DeserializeObject<ViewModelCustomer>(response);
                    var responses = new
                    {
                        KhachHang = customer,
                        HoaDon = oder.OderObj,
                    };

                    return Ok(responses);
                }
            }
            return Ok("Không tìm thấy hóa đơn");


        }
        [HttpGet]
        [Route("GetByIdKhachHang/{Id}")]
        public async Task<IActionResult> GetByIdKhachHang(string Id)
        {
            var oder = await _oderRepo.GetByIdKhachHang(Guid.Parse(Id));
            return Ok(oder);
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var oder = await _oderRepo.GetAll();
            return Ok(oder);
        }
    }
}
