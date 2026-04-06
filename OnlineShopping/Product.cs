using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string SKU { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }=true;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
            
        public virtual ICollection<OrderItem> OrderItems { get; set; }=new HashSet<OrderItem>();
        public Category? Category { get; set; }
    }
}
