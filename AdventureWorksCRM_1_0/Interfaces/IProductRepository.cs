using AdventureWorksCRM_1_0.Models.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksCRM_1_0.Controllers
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        void AddProduct(Product p);
    }
}