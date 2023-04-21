using APIOder.Repositories.IRepo;
using APIOder.ViewModel.OderViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using APIOder.ViewModel.CustomerViewModel;

namespace APIOder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdersController : ControllerBase
    {
        private readonly IOderRepo _oderRepo;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        public OdersController(IOderRepo oderRepo, HttpClient httpClient)
        {
            _oderRepo = oderRepo;
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        [Route("FindCustomer/{phonenumber}")]
        public async Task<IActionResult> FindCustomer(string phonenumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/api/Customers/GetByPhoneNumber/" + phonenumber);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var json = await response.Content.ReadAsStringAsync();
            //if (json == "Không tìm thấy") return Ok(json);

            //var customer = JsonConvert.DeserializeObject<Customer>(json);

            return Ok(json);
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
            var url = "https://localhost:5001/api/Customers/CreateCustomer/";

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(create), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return Ok(responseString);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }

        }
        [HttpGet]
        [Route("GetByIdOder/{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var oder = await _oderRepo.GetByIdOder(Guid.Parse(Id));
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/api/Customers/GetById/" + oder.OderObj.IdCustomer.ToString());

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var json = await response.Content.ReadAsStringAsync();
            if (json == "Không tìm thấy khách hàng")
            {
                var responses = new
                {
                    KhachHang = json,
                    HoaDon = oder.OderObj,
                };

                return Ok(responses);
            }
            else
            {
                var customer = JsonConvert.DeserializeObject<ViewModelCustomer>(json);
                var responses = new
                {
                    KhachHang = customer,
                    HoaDon = oder.OderObj,
                };

                return Ok(responses);
            }

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
