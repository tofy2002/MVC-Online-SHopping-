using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using OnlineShopping.Repo;

namespace MvcProject.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        IUnitofWork unitofwork;
        IEntityRep<Address> addressRepo;
        UserManager<Customer> userManager;

        public AddressController(IUnitofWork _unitofwork, UserManager<Customer> _userManager)
        {
            unitofwork = _unitofwork;
            addressRepo = _unitofwork.AddressRepo;
            userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var addresses = await addressRepo.FindAll(a => a.UserId == userId);

            var vm = addresses.Select(a => new AddressVM
            {
                AddressId = a.AddressId,
                Country = a.Country,
                City = a.City,
                Street = a.Street,
                ZIP = a.ZIP,
                IsDefault = a.IsDefault
            }).ToList();

            return View(vm);
        }
        public IActionResult Create()
        {
            string userId = userManager.GetUserId(User)!;
            var vm = new AddressVM();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddressVM vm)
        {
            string userId = userManager.GetUserId(User)!;
            if (ModelState.IsValid)
            {
                if (vm.IsDefault)
                { var alladdresses = addressRepo.FindAllQueryable(q => q.IsDefault);
                    foreach (var isdef in alladdresses)
                    {
                        isdef.IsDefault=false;
                    }
                }
                Address add = new Address
                {
                    AddressId = vm.AddressId,
                    City = vm.City,
                    Street = vm.Street,
                    ZIP=vm.ZIP,
                    UserId=userId,
                    IsDefault=vm.IsDefault,
                    Country=vm.Country,

                };
                await addressRepo.Add(add);
                await unitofwork.SaveChanges();
            }
            else
            {
                return View(vm);
            }


                return RedirectToAction("Checkout","Order");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            var address=await addressRepo.GetById(id.Value);
            if (address == null) {
                return NotFound();
            }
            var userId = userManager.GetUserId(User);
            if (address.UserId != userId) return Forbid();
            var vm = new AddressVM()
            {
                AddressId = address.AddressId,
                City = address.City,
                Street = address.Street,
                ZIP = address.ZIP,
                IsDefault = address.IsDefault,
                Country = address.Country
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddressVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            var userId = userManager.GetUserId(User);

            var address = await addressRepo.GetById(vm.AddressId);
            if (address == null) return NotFound();
            if (address.UserId != userId) return Forbid();
            if (vm.IsDefault)
            {
                var alladdresses = addressRepo.FindAllQueryable(q => q.IsDefault);
                foreach (var isdef in alladdresses)
                {
                    isdef.IsDefault = false;
                }
            }
            address.Country = vm.Country;
            address.City = vm.City;
            address.Street = vm.Street;
            address.ZIP = vm.ZIP;
            address.IsDefault = vm.IsDefault;

            addressRepo.Update(address);
            await unitofwork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null)
            {
                return BadRequest();
            }
           var address= await addressRepo.GetById(id.Value);
            if (address == null)
            {
                return NotFound();
            }
            var userId = userManager.GetUserId(User);
            if (address.UserId != userId) return Forbid();
            var vm = new AddressVM
            {
                AddressId = address.AddressId,
                Country = address.Country,
                City = address.City,
                Street = address.Street,
                ZIP = address.ZIP,
                IsDefault = address.IsDefault,
                IsDeleted = address.IsDeleted
            };
            return View(vm);
        }   
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id) {
            if (id == null) { return BadRequest(); }
            var address = await addressRepo.GetById(id.Value);
            var userId = userManager.GetUserId(User);
            if (address == null || address.UserId != userId) return Forbid();   
            address.IsDeleted = true;
            await unitofwork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
