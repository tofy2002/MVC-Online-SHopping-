using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using OnlineShopping.Repo;

namespace MvcProject.Controllers
{
    public class CatalogController : Controller
    {
         IUnitofWork unitofWork;
         IEntityRep<Product> productRepo;
         IEntityRep<Category> categoryRepo;
        public CatalogController(IUnitofWork _unitofWork)
        {
            unitofWork = _unitofWork;
            productRepo = _unitofWork.ProductRepo;
            categoryRepo = _unitofWork.CategoryRepo;
        }
        public async Task<IActionResult> Index(int? CategId, string Search, string Sort, int page = 1)
        {
            int pagesize = 10;

            var products = productRepo.FindAllQueryable(q => true).Include("Category");
            if (CategId != null)
            {
                products = products.Where(c => c.CategoryId == CategId);
            }
            if (Search != null) { 
                products=products.Where(c=>c.Name.ToLower().Contains(Search.ToLower())) ;
            }
            products = Sort switch
            {
                "price_asc" => products.OrderBy(c => c.Price),
                "price_desc" => products.OrderByDescending(c => c.Price),
                "Latest"=>products.OrderBy(c=>c.CreatedAt),
                _ => products.OrderBy(c => c.Name)
            };
            var totalitems =await products.CountAsync();
            var NoOfPages=Math.Ceiling((double)totalitems/pagesize);
            var PagePrd = await products
               .Skip((page - 1) * pagesize).Take(pagesize)
               .Select(p => new ProductDetailsVM
               {
                   ProductId = p.ProductId,
                   CategoryId = p.CategoryId,
                   Name = p.Name,
                   SKU = p.SKU,
                   Price = p.Price,
                   StockQuantity = p.StockQuantity,
                   IsActive = p.IsActive,
                   CreatedAt = p.CreatedAt,
                   CategoryName=p.Category.Name
               }).ToListAsync();

            var categories=await categoryRepo.GetAll();


            ProductListVM vm = new ProductListVM
            {

                Products = PagePrd,
                Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value=c.CategoryId.ToString()
                }).ToList(),
                CategId = CategId,
                Search = Search,
                TotalPages = (int)NoOfPages,
                PageIndex = page,
                Sort = Sort,
                TotalCount = totalitems


            };
            return View(vm);
        }
        public async Task<IActionResult> Details(int? id) 
        { 
            if(id == null)
            {
                return BadRequest();
            }
            //var product = await productRepo.GetById(id.Value);
            var product = productRepo.FindAllQueryable(p=>p.ProductId == id.Value).Include("Category").FirstOrDefault();
            if (product == null)
                return NotFound();
            ProductDetailsVM vm = new ProductDetailsVM
            {
                ProductId= product.ProductId,
                CategoryId=product.CategoryId,
                Name=product.Name,
                SKU = product.SKU,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CreatedAt=product.CreatedAt,
                CategoryName=product.Category.Name
            };
            return View(vm);
        }
        
       

    }
}
