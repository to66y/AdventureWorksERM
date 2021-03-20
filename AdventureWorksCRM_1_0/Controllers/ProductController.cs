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

        public ProductController(IQueryable<Product> prodRepo, IQueryable<ProductCategory> catRepo)
        {
            ProductRepository = prodRepo;
            CategoryRepository = catRepo;
        }
        public async Task<IActionResult> Index(string category = "", int page = 1)
        {
            ViewBag.Category = CategoryRepository;
            var queue = await PagedList<Product>.AsPagedAsync(ProductRepository, pageIndex: page, pageSize: 7);
            return View(queue);
        }
    }
}
