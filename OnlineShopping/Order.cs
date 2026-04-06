using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Order
    {
         public enum OrderStatus
    {
        pending = 0, confirmed = 1, shipping = 2, delivered = 3
    }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int ShippingAddressId { get; set; }
        public string OrderNo { get; set; }
        [Column(TypeName = "Date")]
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }=0;
        public decimal TotalAmount { get; set; }
        public Customer Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }=new HashSet<OrderItem>();

    }
}
