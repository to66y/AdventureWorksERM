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
        public IRepository<Product> ProductRepository { get; }
        public IRepository<ProductCategory> CategoryRepository { get; }

        public ProductController(IRepository<Product> prodRepo, IRepository<ProductCategory> catRepo)
        {
            ProductRepository = prodRepo;
            CategoryRepository = catRepo;
        }
        public async Task<IActionResult> Index(string category="", int page = 1)
        {
            ViewBag.Category = CategoryRepository.Storage.Where(pc => pc.Name.Contains(category)).ToArray();
            var queue = await PagedList<Product>.AsPagedAsync(ProductRepository.Storage, pageIndex: page, pageSize: 7);
            return View(queue);
        }
    }
}
