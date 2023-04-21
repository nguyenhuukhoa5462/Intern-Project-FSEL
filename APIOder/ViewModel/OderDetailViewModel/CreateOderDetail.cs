using System.ComponentModel.DataAnnotations;

namespace APIOder.ViewModel.OderDetailViewModel
{
    public class CreateOderDetail
    {
        [Required(ErrorMessage = "Vui lòng nhập Id hóa đơn")]
        public Guid OderId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        public decimal UnitPrice { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public int Quantity { get; set; }
    }
}
