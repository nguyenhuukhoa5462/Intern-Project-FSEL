namespace APIOder.ViewModel.OderViewModel
{
    public class ViewModelOder
    {
        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public DateTime OderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<ViewModelOderDetail>? ListOderDetail { get; set; }
    }
}
