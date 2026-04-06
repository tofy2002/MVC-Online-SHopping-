using OnlineShopping;
using System.ComponentModel.DataAnnotations;
namespace MvcProject.Models.ViewModel
{
    public class CheckoutVM
    {
        public List<AddressVM> Addresses { get; set; }= new List<AddressVM>();
        public CartVM cart {  get; set; } = new CartVM();
        [Required(ErrorMessage = "Please select a shipping address")]
        public  int SelectedAddressId {  get; set; }
      
    }
}
