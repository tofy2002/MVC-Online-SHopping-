
namespace MvcProject.Models.ViewModel
{
    public class CartVM
    {
        public List<CartItemVM> CartItems { get; set; }=new List<CartItemVM>();
        public decimal TotalPrice { get; set; } = 0;
        public int TotalItems { get; set; }
     
    }
}
