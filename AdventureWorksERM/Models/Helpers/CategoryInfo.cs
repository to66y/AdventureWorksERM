using AdventureWorksERM.Models.DbContexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AdventureWorksERM.Models.Helpers
{
    public class CategoryInfo
    {
        public CategoryInfo(List<ProductCategory> categories, int? category)
        {
            categories.Insert(0, new ProductCategory { Name = "All", ProductCategoryId = 0 });
            Categories = new SelectList(categories, "ProductCategoryId", "Name", category);
            SelectedCategory = category;
        }
        public SelectList Categories { get; private set; }
        public int? SelectedCategory { get; private set; }
    }
}
