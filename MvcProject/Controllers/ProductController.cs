using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using OnlineShopping.Data;
using OnlineShopping.Repo;
using SQLitePCL;


namespace MvcProject.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProductController : Controller
    {
        IUnitofWork unitofWork { get; set; }
        IEntityRep<Product> ProductRepo { get; set; }
        IEntityRep<Category> CategoryRepo { get; set; }
        
        public ProductController(IUnitofWork _unitofWork)
        {
            unitofWork= _unitofWork;
            ProductRepo = _unitofWork.ProductRepo;
            CategoryRepo = _unitofWork.CategoryRepo;
        }
        //use view model
        public async Task<IActionResult> Index()
        {
            var model=await ProductRepo.GetAll("Category");
            var DetailsVM =  model.Select(p=>new ProductDetailsVM
            {
                ProductId=p.ProductId,
                Name=p.Name,
                Price=p.Price,
                StockQuantity=p.StockQuantity,
                CategoryId=p.CategoryId,
                SKU=p.SKU,
                CategoryName = p.Category?.Name
            }).ToList();
            return View(DetailsVM);
        }
        public async Task<IActionResult> Create()
        {
            var catogries = await CategoryRepo.GetAll();
            var vm = new ProductVM
            {
                Categories = catogries.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString()

                }).ToList()
            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                Product prod = new Product
                {
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    SKU = Guid.NewGuid().ToString().Substring(0, 8)

                };
                await ProductRepo.Add(prod);
                await unitofWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var catogries = await CategoryRepo.GetAll();
            product.Categories = catogries.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();
            return View(product);

        }
        public async Task<IActionResult> Edit(int id)
        {
            var model = await ProductRepo.GetById(id);

            if (model == null)
                return NotFound();

            var categories = await CategoryRepo.GetAll();

            ProductUpdateVM vm = new ProductUpdateVM
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId,
                SKU = model.SKU,

                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateVM vm)
        {
            if (ModelState.IsValid)
            {
                var model = await ProductRepo.GetById(vm.ProductId);

                if (model == null)
                    return NotFound();

                model.Name = vm.Name;
                model.Price = vm.Price;
                model.StockQuantity = vm.StockQuantity;
                model.CategoryId = vm.CategoryId;
                model.SKU = vm.SKU;
                //tracking conflict happen if we use update 
                await unitofWork.SaveChanges();

                return RedirectToAction("Index");
            }

            var categories = await CategoryRepo.GetAll();

            vm.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

            return View(vm);
        }
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model= await ProductRepo.GetById(id.Value);
            if(model== null)
            {
                return NotFound();
            }
            return View(model);

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await ProductRepo.GetById(id);
            if (model == null)
            {
                return NotFound();
            }
            model.IsDeleted= true;
            await unitofWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
