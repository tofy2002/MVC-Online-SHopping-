using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping;
namespace MvcProject.Models.ViewModel
{
    public class ProductListVM
    {
        public List<ProductDetailsVM> Products { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int? CategId { get; set; }
        public string? Search { get; set; } 
        public int TotalPages { get; set; }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; } 
        public int TotalCount { get; set; }
       

    }
}
