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
using Microsoft.Data.SqlClient;

namespace AdventureWorksERM.Controllers
{
    public class ProductController : Controller
    {
        private AdventureWorksContext _context;

        public ProductController(AdventureWorksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sort, int? category, int page = 1)
        {
            var source = _context.Products.Include("ProductModel")
                             .Include("ProductSubcategory");

            if (category != null && category != 0)
            {
                source = _context.Products.Where(x => x.ProductSubcategory.ProductCategoryId == category)
                            .Include("ProductModel")
                            .Include("ProductSubcategory");
            }

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sort == "Price" ? "price_desc" : "Price";
            ViewData["CostSortParm"] = sort == "Cost" ? "cost_desc" : "Cost";
            switch (sort)
            {
                case "name_desc":
                    source = source.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    source = source.OrderBy(s => s.ListPrice);
                    break;
                case "price_desc":
                    source = source.OrderByDescending(s => s.ListPrice);
                    break;
                case "Cost":
                    source = source.OrderBy(s => s.StandardCost);
                    break;
                case "cost_desc":
                    source = source.OrderByDescending(s => s.StandardCost);
                    break;
                default:
                    source = source.OrderBy(s => s.Name);
                    break;
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
