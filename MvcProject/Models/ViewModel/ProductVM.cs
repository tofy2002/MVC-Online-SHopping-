using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models.ViewModel
{
    public class ProductVM
    {
        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999.99)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, 100000)]
        public int StockQuantity { get; set; }
        //public string SKU { get; set; } = "";
        public List<SelectListItem>? Categories { get; set; }
    }
}
