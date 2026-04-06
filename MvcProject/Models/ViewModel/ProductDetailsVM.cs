namespace MvcProject.Models.ViewModel
{
    public class ProductDetailsVM
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string SKU { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }=true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CategoryName { get; set; } = "";
    }
}
