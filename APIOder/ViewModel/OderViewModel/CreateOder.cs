using APIOder.ViewModel.OderDetailViewModel;
using System.ComponentModel.DataAnnotations;

namespace APIOder.ViewModel.OderViewModel
{
    public class CreateOder
    {
        [Required(ErrorMessage = "Vui lòng nhập Id khách hàng")]
        public Guid IdCustomer { get; set; }
        public List<OderDetailCreates> OderDetail { get; set; }
    }
}
