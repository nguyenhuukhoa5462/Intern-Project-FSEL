namespace APIOder.Models
{
    public class OderDetail
    {
        public Guid Id { get; set; }
        public Guid OderId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Oder Oder { get; set; }
    }
}
