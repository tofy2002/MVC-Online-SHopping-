using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Customer : IdentityUser
	{
        public string FullName { get; set; }

        public ICollection<Order> Orders { get; set; } 
        public ICollection<Address> Addresses { get; set; } 
    }
}
