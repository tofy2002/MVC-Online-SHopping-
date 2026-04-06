using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models.ViewModel
{
    public class AddressVM
    {
        public int AddressId { get; set; }
        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; }
        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }
        [Required(ErrorMessage = "Street is required")]
        [StringLength(100, ErrorMessage = "Street cannot exceed 100 characters")]
        public string Street { get; set; }
        [Required(ErrorMessage = "ZIP code is required")]
        [StringLength(20, ErrorMessage = "ZIP code cannot exceed 20 characters")]
        public string ZIP { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }=false;
    }
}
