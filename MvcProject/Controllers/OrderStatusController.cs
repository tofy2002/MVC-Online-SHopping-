using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping;
using OnlineShopping.Repo;
using System.Data;
using static OnlineShopping.Order;

namespace MvcProject.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]
    public class OrderStatusController : Controller
    {
        IUnitofWork unitofwork;
        IEntityRep<Product> productrepo;
        IEntityRep<Address> addressRepo;
        IEntityRep<Order> orderRepo;
        IEntityRep<OrderItem> orderItemRepo;
        UserManager<Customer> userManager;

        public OrderStatusController(IUnitofWork _unitofwork, UserManager<Customer> _userManager)
        {
            unitofwork = _unitofwork;
            productrepo = _unitofwork.ProductRepo;
            addressRepo = _unitofwork.AddressRepo;
            orderRepo = _unitofwork.OrderRepo;
            orderItemRepo = _unitofwork.OrderItemRepo;
            userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            var orders=await orderRepo.GetAll("Customer");
            ViewBag.StatusList = new SelectList(
            Enum.GetValues(typeof(OrderStatus))
             );

            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderid, OrderStatus status)
        {
            var order=await orderRepo.GetById(orderid);
            if (order == null)
                return NotFound();
            order.Status=status;
            await unitofwork.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
