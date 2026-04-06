using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using OnlineShopping.Repo;
using System.Text.Json;

namespace MvcProject.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {   
        IUnitofWork unitofwork;
        IEntityRep<Product> productrepo;
        IEntityRep<Address> addressRepo;
        IEntityRep<Order> orderRepo;
        IEntityRep<OrderItem> orderItemRepo;
        UserManager<Customer> userManager;

        public OrderController(IUnitofWork _unitofwork, UserManager<Customer> _userManager)
        {
            unitofwork = _unitofwork;
            productrepo = _unitofwork.ProductRepo;
            addressRepo = _unitofwork.AddressRepo;
            orderRepo = _unitofwork.OrderRepo;
            orderItemRepo = _unitofwork.OrderItemRepo;
            userManager = _userManager; 
        }
        public async Task<IActionResult> Checkout()
        {
            CartVM cart;
            var cartdata= HttpContext.Session.GetString("Cart");
            if (cartdata == null) 
            {
                return RedirectToAction("Index","Cart");  
            }
            else
            {
                cart = JsonSerializer.Deserialize<CartVM>(cartdata)!;
            }
            var userId = userManager.GetUserId(User);

            var UserAddresses=await addressRepo.FindAll(c=>c.UserId == userId);
            if(!UserAddresses.Any())
            {
                return RedirectToAction("Create", "Address");
            }
            CheckoutVM vm=new CheckoutVM();
            vm.cart = cart;

            foreach (var address in UserAddresses)
            {
                AddressVM addressVM=new AddressVM();
                addressVM.AddressId = address.AddressId;
                addressVM.City = address.City;
                addressVM.ZIP = address.ZIP;
                addressVM.Street = address.Street;
                addressVM.Country = address.Country;
                addressVM.IsDefault = address.IsDefault;
                vm.Addresses.Add(addressVM);
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutVM vm)
        {

            var cartdata = HttpContext.Session.GetString("Cart");
            if (cartdata == null)
            {
                return RedirectToAction("Index", "Cart");
            }
            var cart = JsonSerializer.Deserialize<CartVM>(cartdata)!;
            foreach (var qty in cart.CartItems)
            {
                var prodid = qty.ProductId;
                var product = await productrepo.GetById(prodid);
                if (product == null || product.StockQuantity<qty.Quantity || !product.IsActive) {
                    TempData["Error"] = $"{qty.ProductName} is out of stock or unavailable.";
                    return RedirectToAction("Index", "Cart");
                }
                

            }
            using var transaction = await unitofwork.BeginTransactionAsync();
            try
            {
                var userId = userManager.GetUserId(User);
                Order order = new Order
                {
                    UserId = userId,
                    ShippingAddressId = vm.SelectedAddressId,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.TotalPrice,
                    Status = 0,
                    OrderNo = Guid.NewGuid().ToString().Substring(0, 8)

                };
                await orderRepo.Add(order);
                await unitofwork.SaveChanges();
                foreach (var item in cart.CartItems)
                {
                    var product = await productrepo.GetById(item.ProductId);
                    OrderItem orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        OrderId = order.OrderId,
                        UnitPrice = item.Price,
                        Quantity = item.Quantity,
                        LineTotal = item.Quantity * item.Price
                    };
                    await orderItemRepo.Add(orderItem);

                    product.StockQuantity -= item.Quantity;
                }

                await unitofwork.SaveChanges();
                await transaction.CommitAsync();
                HttpContext.Session.Remove("Cart");

                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
                TempData["Error"] = "Checkout failed";
                return RedirectToAction("Index", "Cart");
            }

        }
        //use view model
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var orders= await orderRepo.FindAll(c=> c.UserId==userId);
            var ordervm = orders.Select(vm => new OrderVM
            {
                OrderID=vm.OrderId,
                dateTime=vm.OrderDate,
                TotalPriced=vm.TotalAmount,
                Status=vm.Status,
                OrderNo=vm.OrderNo

            }).ToList();

            return View(ordervm);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            var userId = userManager.GetUserId(User);
            //string name = userManager.GetUserId(User);
            var order = await orderRepo.GetById(id.Value);
            if (order == null) return NotFound();
            if (order.UserId != userId) return Forbid();
            var address = addressRepo.FindAllQueryable(q => q.AddressId == order.ShippingAddressId).IgnoreQueryFilters().FirstOrDefault();
          //  var address = await addressRepo.GetById(order.ShippingAddressId);
            var customer = await userManager.FindByIdAsync(userId);
            var orderitem = orderItemRepo.FindAllQueryable(
                  o => o.OrderId == id.Value
      
              ).IgnoreQueryFilters().Include("Product");
        
            var itemsVM = orderitem.Select(i => new OrderItemDetailsVM
            {
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList();
            OrderDetailsVM orderDetailsVM = new OrderDetailsVM
            {
                OrderNo = order.OrderNo,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalAmount,

                Address = new AddressVM
                {
                    Country = address.Country,
                    City = address.City,
                    Street = address.Street,
                    ZIP = address.ZIP,
                    AddressId = address.AddressId,
                    IsDefault = address.IsDefault,
                    IsDeleted = address.IsDeleted
                },

                Customer = new CustomerVM
                {
                    CustomerName = customer.FullName,
                    CustomerEmail = customer.Email
                },

                items = itemsVM

            };
            return View(orderDetailsVM);


        }
    }
}
