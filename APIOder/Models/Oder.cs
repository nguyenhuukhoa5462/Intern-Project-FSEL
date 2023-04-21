namespace APIOder.Models
{
    public class Oder
    {
        public Oder()
        {
            OderDetails = new HashSet<OderDetail>();
        }

        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public DateTime OderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public virtual ICollection<OderDetail> OderDetails { get; set; }
    }
}
