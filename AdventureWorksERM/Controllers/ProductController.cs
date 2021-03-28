using AdventureWorksERM.Models.DbContexts;
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
        private readonly AdventureWorksContext _context;

        public ProductController(AdventureWorksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string orderby, int? category, string search = "", int page = 1)
        {
            IQueryable<Product> source;
            IQueryable<ProductPhoto> photos;

            if (category != null && category != 0)
            {
                source = _context.Products.Where(x => x.ProductSubcategory.ProductCategoryId == category)
                    .Include(p => p.ProductModel)
                    .Include(p => p.ProductSubcategory)
                    .Include(p => p.ProductProductPhotos)
                    .ThenInclude(p => p.ProductPhoto);
            }
            else
            {
                source = _context.Products
                    .Include(p => p.ProductModel)
                    .Include(p => p.ProductSubcategory)
                    .Include(p => p.ProductProductPhotos)
                    .ThenInclude(p => p.ProductPhoto);
            }

            photos = _context.ProductPhotos
                .Include(pp => pp.ProductProductPhotos)
                .ThenInclude(pp => pp.Product);

            if (!String.IsNullOrEmpty(search))
            {
                source = source.Where(x => x.Name.Contains(search));
            }

            source = SortByParams(orderby, source);

            var pageSize = 7;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageInfo pageInfo = new(count, page, pageSize);
            CategoryInfo categoryInfo = new(await _context.ProductCategories.ToListAsync(), category);



            ProductsViewModel productsViewModel = new()
            {
                Products = items,
                ProductPhoto = photos,
                PageInfo = pageInfo,
                CategoryInfo = categoryInfo,
                SearchedName = search,
            };

            return View(productsViewModel);
        }

        private IQueryable<Product> SortByParams(string orderby, IQueryable<Product> source)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(orderby) ? "name_desc" : "";
            ViewData["PriceSortParm"] = orderby == "Price" ? "price_desc" : "Price";
            ViewData["CostSortParm"] = orderby == "Cost" ? "cost_desc" : "Cost";
            source = orderby switch
            {
                "name_desc" => source.OrderByDescending(s => s.Name),
                "Price" => source.OrderBy(s => s.ListPrice),
                "price_desc" => source.OrderByDescending(s => s.ListPrice),
                "Cost" => source.OrderBy(s => s.StandardCost),
                "cost_desc" => source.OrderByDescending(s => s.StandardCost),
                _ => source.OrderBy(s => s.Name),
            };
            return source;
        }
    }
}
