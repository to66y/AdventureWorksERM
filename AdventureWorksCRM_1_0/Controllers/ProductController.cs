using AdventureWorksCRM_1_0.Models.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdventureWorksCRM_1_0.Models.Helpers;

namespace AdventureWorksCRM_1_0.Controllers
{
    public class ProductController : Controller
    {
        public IProductRepository Repository { get; }

        public ProductController(IProductRepository repo)
        {
            Repository = repo;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            //var queue = Repository.Products;
            var queue = await PagedList<Product>.AsPagedAsync(Repository.Products, pageIndex: page, pageSize: 7);
            return View(queue);
        }
    }
}
