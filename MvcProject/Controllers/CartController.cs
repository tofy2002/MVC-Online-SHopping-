using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using OnlineShopping.Repo;
using System.Text.Json;

namespace MvcProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        IUnitofWork unitofwork;
        IEntityRep<Product> productrepo;


        public CartController(IUnitofWork _unitofwork)
        {
            unitofwork = _unitofwork;
            productrepo = _unitofwork.ProductRepo;
        }
        public IActionResult Index()
        {
            CartVM cart;
            var vald = HttpContext.Session.GetString("Cart");
            if (vald == null)
            {
                cart = new CartVM();
                //return View(cart);
            }
            else
            {
                cart = JsonSerializer.Deserialize<CartVM>(vald)!;
            }

            return View(cart);
        }
        //[HttpPost]
        public async Task<IActionResult> Add(int prodId)
        {
            CartVM cart;
            var cartdata = HttpContext.Session.GetString("Cart");
            if (cartdata == null)
            {
                cart = new CartVM();
            }
            else
            {
                cart = JsonSerializer.Deserialize<CartVM>(cartdata)!;
            }
            var product = await productrepo.GetById(prodId);
            if (product == null || !product.IsActive || product.StockQuantity <= 0)
            {
                TempData["Error"] = "This product is unavailble";
                return RedirectToAction("Index","Catalog");
            }
            var exisit=cart.CartItems.Find(c=>c.ProductId==prodId);
            if(exisit == null)
            {
                CartItemVM vm = new CartItemVM
                {
                    Quantity = 1,
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.Price
                };
                cart.CartItems.Add(vm);
            }
            else
            {
                if (exisit.Quantity >= product.StockQuantity) {
                    TempData["Error"] = "No enough Stock";
                    return RedirectToAction("Index", "Cart");
                }
                exisit.Quantity++;
            }
            cart.TotalPrice = cart.CartItems.Sum(c => c.Price*c.Quantity);
            cart.TotalItems = cart.CartItems.Sum(c => c.Quantity);
            var json=JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart",json);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int itemId, int qty)
        {
            CartVM cart;
            var cartdata = HttpContext.Session.GetString("Cart");
            if (cartdata == null) { return RedirectToAction(nameof(Index)); }
            cart = JsonSerializer.Deserialize<CartVM>(cartdata)!;
            
            var exisit = cart.CartItems.FirstOrDefault(c => c.ProductId == itemId);
            if (exisit == null) { return RedirectToAction(nameof(Index)); }
            if (qty <= 0)
            {
                cart.CartItems.Remove(exisit);
            }
            else
            {   var product=await productrepo.GetById(itemId);
                if (qty > product.StockQuantity)
                {
                    TempData["Error"] = "No enough Stock";
                    return RedirectToAction("Index", "Cart");

                }
                exisit.Quantity = qty;
            }
            cart.TotalPrice = cart.CartItems.Sum(c => c.Price * c.Quantity);
            cart.TotalItems = cart.CartItems.Sum(c => c.Quantity);

            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", json);

            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Remove(int itemId)
        {   
            CartVM cart;
            var cartdata = HttpContext.Session.GetString("Cart");
            if (cartdata == null) { return RedirectToAction(nameof(Index)); }
            cart = JsonSerializer.Deserialize<CartVM>(cartdata)!;

            var exisit = cart.CartItems.FirstOrDefault(c => c.ProductId == itemId);
            if (exisit == null) { return RedirectToAction(nameof(Index)); }

            cart.CartItems.Remove(exisit);
            cart.TotalPrice = cart.CartItems.Sum(c => c.Price * c.Quantity);
            cart.TotalItems = cart.CartItems.Sum(c => c.Quantity);
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", json);
            return RedirectToAction(nameof(Index));
        }
    }
}
