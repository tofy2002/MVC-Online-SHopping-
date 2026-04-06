using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace MvcProject.Models.ViewModel
{
    public class ProductUpdateVM

    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        public string SKU { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
