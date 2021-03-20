using AdventureWorksERM.Models.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdventureWorksERM.Models.Helpers;

namespace AdventureWorksERM.Controllers
{
    public class ProductController : Controller
    {
        public IQueryable<Product> ProductRepository { get; }
        public IQueryable<ProductCategory> CategoryRepository { get; }
        private AdventureWorksContext _context;

        public ProductController(AdventureWorksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string category = "", int page = 1)
        {
            ViewBag.Category = _context.ProductCategories.Where(x=> x.Name.Contains(category)).FirstOrDefault();
            ProductCategory cat = ViewBag.Category;
            var products = _context.Products.Where(x=>x.ProductSubcategory.ProductCategoryId==cat.ProductCategoryId)
                .Include("ProductModel")
                .Include("ProductSubcategory");
            var result = await PagedList<Product>.AsPagedAsync(products, pageIndex: page, pageSize: 7);
            return View(result);
        }
    }
}
