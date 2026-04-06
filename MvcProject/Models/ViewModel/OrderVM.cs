using static OnlineShopping.Order;

namespace MvcProject.Models.ViewModel
{
    public class OrderVM
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime dateTime { get; set; }
        public decimal TotalPriced { get; set; }
        public OrderStatus Status { get; set; }
    }
}
