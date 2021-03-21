using AdventureWorksERM.Models.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdventureWorksERM.Models.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdventureWorksERM.Models.Production.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AdventureWorksERM.Controllers
{
    public class ProductController : Controller
    {
        private AdventureWorksContext _context;

        public ProductController(AdventureWorksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? category, int page = 1)
        {
            var source = _context.Products.Include("ProductModel")
                             .Include("ProductSubcategory");

            if (category != null && category != 0)
            {
                source = _context.Products.Where(x => x.ProductSubcategory.ProductCategoryId == category)
                            .Include("ProductModel")
                             .Include("ProductSubcategory");
            }
            var pageSize = 10;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageInfo pageInfo = new PageInfo(count, page, pageSize);
            CategoryInfo categoryInfo = new CategoryInfo(await _context.ProductCategories.ToListAsync(), category);
            ProductsViewModel productsViewModel = new ProductsViewModel
            {
                Products = items,
                PageInfo = pageInfo,
                CategoryInfo = categoryInfo,
            };

            return View(productsViewModel);
        }
    }
}
