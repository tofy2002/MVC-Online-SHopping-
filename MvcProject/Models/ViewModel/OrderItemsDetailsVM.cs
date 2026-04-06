namespace MvcProject.Models.ViewModel
{
    public class OrderItemDetailsVM
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
