using APIOder.Repositories.IRepo;
using APIOder.ViewModel.OderDetailViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIOder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OderDetailsController : ControllerBase
    {
        private readonly IOderDetailRepo _repo;

        public OderDetailsController(IOderDetailRepo repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOderDetail create)
        {
            var result = await _repo.Create(create);
            if (result == 0) return BadRequest("Không tìm thấy hóa đơn");
            if (result == 1) return BadRequest("Update thành công");
            if (result == 3) return BadRequest("Đã xảy ra lỗi, vui lòng thử lại");
            return Ok("Thêm thành công");
        }
        [HttpGet]
        [Route("GetByIdOder/{Id}")]
        public async Task<IActionResult> GetListOderDetailByIdOder(string Id)
        {
            var result = await _repo.GetByIdOder(Guid.Parse(Id));
            return Ok(result);
        }
    }
}
