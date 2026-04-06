using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Address
    {
        public int AddressId { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZIP { get; set; }
        public bool IsDefault { get; set; } 
        public Customer Customer { get; set; }
        public virtual ICollection<Order> Orders { get;set; }
    }
}
