using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping;
using System.ComponentModel.DataAnnotations;
namespace MvcProject.Models.ViewModel
{
    public class CategoryEditVM
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
