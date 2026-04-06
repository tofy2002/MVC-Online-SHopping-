using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping;

namespace MvcProject.Models.ViewModel
{
    public class CategoryEditVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
