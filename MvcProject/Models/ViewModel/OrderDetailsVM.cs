 using OnlineShopping;
using static OnlineShopping.Order;

namespace MvcProject.Models.ViewModel
{
    public class OrderDetailsVM
    {
        public string OrderNo { get; set; }
        public int shippingAddressId { get; set; }
        public AddressVM Address { get; set; }=new AddressVM();
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } = 0;
        public CustomerVM Customer { get; set; }=new CustomerVM();
        public decimal TotalPrice { get; set; }
        public List<OrderItemDetailsVM> items { get; set; }= new List<OrderItemDetailsVM>();

    }
}
