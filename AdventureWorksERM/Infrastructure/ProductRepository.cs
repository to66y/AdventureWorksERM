using AdventureWorksERM.Controllers;
using AdventureWorksERM.Models.DbContexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksERM.Infrastructure
{
    public class ProductRepository : IRepository<Product>
    {
        public IQueryable<Product> Storage => _context.Products.Include("ProductModel").Include("ProductSubcategory");

        private readonly AdventureWorksContext _context;
        public ProductRepository(AdventureWorksContext context) => _context = context;
        public void Add(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products;
        }
    }
}
