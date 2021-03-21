using AdventureWorksERM.Controllers;
using AdventureWorksERM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AdventureWorksERM.Models.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksERM.Models.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksERM_Test
{
    public class ProductControllerTest
    {
        private AdventureWorksContext InitTestDB()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseInMemoryDatabase(databaseName: "AWListDatabase" + DateTime.Now.ToFileTimeUtc())
                .Options;

            var context = new AdventureWorksContext(options);
            context.Products.Add(new Product { Name = "Prod1", Class = "Class1", ProductSubcategoryId = 1 });
            context.Products.Add(new Product { Name = "Prod2", Class = "Class2", ProductSubcategoryId = 2 });
            context.Products.Add(new Product { Name = "Prod3", Class = "Class3", ProductSubcategoryId = 3 });

            context.ProductCategories.Add(new ProductCategory { Name = "Cat1", ProductCategoryId = 1, });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat2", ProductCategoryId = 2, });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat3", ProductCategoryId = 3, });

            context.ProductSubcategories.Add(new ProductSubcategory { Name = "SubCat1", ProductSubcategoryId = 1, ProductCategoryId = 1 });
            context.ProductSubcategories.Add(new ProductSubcategory { Name = "SubCat2", ProductSubcategoryId = 2, ProductCategoryId = 2 });
            context.ProductSubcategories.Add(new ProductSubcategory { Name = "SubCat3", ProductSubcategoryId = 3, ProductCategoryId = 3 });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void HasProductController()
        {
            var context = InitTestDB();
            ProductController controller = new ProductController(context);
            Assert.NotNull(controller);
        }

        [Fact]
        public async void ProductControllerReturnProduct()
        {
            var context = InitTestDB();
            ProductController controller = new ProductController(context);
            var viewResult = (await controller.Index() as ViewResult)?.ViewData.Model as PagedList<Product>;
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void ProductControllerHasViewBagCategory()
        {
            var context = InitTestDB();
            ProductController controller = new ProductController(context);
            var result = Task.FromResult(controller.Index());
            var category = controller.ViewBag.Category;
            Assert.NotNull(category);
        }

        [Fact]
        public async void ProductControllerFilterByCategory()
        {
            var context = InitTestDB();
            ProductController controller = new ProductController(context);
            string category = "Cat1";
            var model = (await controller.Index(category) as ViewResult)?.ViewData.Model as PagedList<Product>;
            Assert.Equal("Prod1", model.First().Name);
        }
    }
}
