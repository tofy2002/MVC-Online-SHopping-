using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category>? SubCategories { get; set; } =new List<Category>();
        public bool IsDeleted { get; set; }=false;
    }
}
