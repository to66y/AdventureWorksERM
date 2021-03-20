using AdventureWorksERM.Controllers;
using AdventureWorksERM.Models.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksERM.Infrastructure
{
    public class CategoriesRepository : IRepository<ProductCategory>
    {
        public IEnumerable<ProductCategory> Storage => _context.ProductCategories.ToArray();

        private AdventureWorksContext _context;

        public CategoriesRepository(AdventureWorksContext context) => _context = context;

        public void Add(ProductCategory p)
        {
            _context.Add(p);
            _context.SaveChanges();
        }
    }
}
