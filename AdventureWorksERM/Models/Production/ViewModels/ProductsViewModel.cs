using AdventureWorksERM.Models.DbContexts;
using AdventureWorksERM.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksERM.Models.Production.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductPhoto> ProductPhoto { get; set; }
        public string SearchedName { get; set; }
        public PageInfo PageInfo { get; set; }
        public CategoryInfo CategoryInfo { get; set; }
    }
}
