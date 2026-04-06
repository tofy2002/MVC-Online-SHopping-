using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using OnlineShopping.Data;
using OnlineShopping.Repo;

namespace MvcProject.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        IUnitofWork unitofWork;
        IEntityRep<Category> categoryRepo;
        IEntityRep<Product> ProductRepos;
        public CategoryController(IUnitofWork _unitofWork)
        {
            unitofWork = _unitofWork;
            categoryRepo = _unitofWork.CategoryRepo;
            ProductRepos = _unitofWork.ProductRepo;

        }
        public async Task<IActionResult> Index()
        {
            var model = await categoryRepo.GetAll("ParentCategory");
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await categoryRepo.GetAll();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
  
        public async Task<IActionResult> Create(CategoryEditVM category)
        {
            if (ModelState.IsValid)
            {
                Category vm = new Category
                {
                    Name = category.Name,
                    ParentCategoryId=category.ParentCategoryId
                };
               await categoryRepo.Add(vm);
               await unitofWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.categories = await categoryRepo.GetAll();
            return View(category);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var category = await categoryRepo.GetById(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            var catogries = await categoryRepo.GetAll();
            CategoryEditVM vm = new CategoryEditVM
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                Categories = catogries.Where(c => c.CategoryId != id).Select(c => new SelectListItem{
                    Value = c.CategoryId.ToString(),
                    Text=c.Name
                })
            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryEditVM vm)
        {
            if (ModelState.IsValid)
            {
                var category = await categoryRepo.GetById(id);
                category.Name = vm.Name;
                category.ParentCategoryId = vm.ParentCategoryId;
               // categoryRepo.Update(category);
                await unitofWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var categories = await categoryRepo.GetAll();
            vm.Categories = categories
                .Where(c => c.CategoryId != vm.CategoryId)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                });
            return View(vm);
        }
        public async Task<IActionResult> Delete(int? id) { 
            if(id == null)
            {
                return BadRequest();
            }
            var category=await categoryRepo.GetById(id.Value);
            if (category == null) { 
                return NotFound();
            }
            CategoryEditVM vm = new CategoryEditVM
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
            return View(vm);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConformed(int? id)
        {
            if (id == null) { return BadRequest(); }
            var category = await categoryRepo.GetById(id.Value);
            if (category == null) return NotFound();
            category.IsDeleted = true;
            await unitofWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
