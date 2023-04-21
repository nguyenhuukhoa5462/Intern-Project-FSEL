using System.ComponentModel.DataAnnotations;

namespace APIOder.ViewModel.OderDetailViewModel
{
    public class OderDetailCreates
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        public decimal UnitPrice { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public int Quantity { get; set; }
    }
}
