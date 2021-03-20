using AdventureWorksERM.Controllers;
using AdventureWorksERM.Models.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections;

namespace AdventureWorksERM.Infrastructure
{
    public class ProductRepository : IRepository<Product>
    {
        public IEnumerable<Product> Storage => _context.Products.Include("ProductModel").Include("ProductSubcategory").ToArray();

        public readonly AdventureWorksContext _context;
        public ProductRepository(AdventureWorksContext context) => _context = context;
        public void Add(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
        }
    }
}
