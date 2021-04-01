using AdventureWorksERM.Controllers;
using AdventureWorksERM.Models.DbContexts;
using System.Linq;

namespace AdventureWorksERM.Infrastructure
{
    public class CategoriesRepository : IRepository<ProductCategory>
    {
        public IQueryable<ProductCategory> Storage => _context.ProductCategories;

        private AdventureWorksContext _context;

        public CategoriesRepository(AdventureWorksContext context) => _context = context;

        public void Add(ProductCategory p)
        {
            _context.Add(p);
            _context.SaveChanges();
        }

        public IQueryable<ProductCategory> GetAll()
        {
            return _context.ProductCategories;
        }
    }
}
