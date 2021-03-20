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

namespace AdventureWorksCRM_1_0_Test
{
    public class ProductControllerTest
    {
        public AdventureWorksContext context;
        public Product[] ProductForCompare = new Product[]
        {
            new Product { Name = "Prod1", Class = "Class1" },
            new Product { Name = "Prod2", Class = "Class2" },
            new Product { Name = "Prod3", Class = "Class3" },
        };
        
        public ProductCategory[] ProductCategoryForCompare = new ProductCategory[]
        {
            new ProductCategory { Name = "Cat1", },
            new ProductCategory { Name = "Cat2", },
            new ProductCategory { Name = "Cat3", },
        };

        private void InitTestDB()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseInMemoryDatabase(databaseName: "AWListDatabase")
                .Options;

            context = new AdventureWorksContext(options);
            context.Products.Add(new Product { Name = "Prod1", Class = "Class1" });
            context.Products.Add(new Product { Name = "Prod2", Class = "Class2" });
            context.Products.Add(new Product { Name = "Prod3", Class = "Class3" });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat1" });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat2" });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat3" });

            context.SaveChanges();
        }
        private ProductController GetFakeProductController()
        {
            InitTestDB();
            return new ProductController(context);//.Products, context.ProductCategories);
        }



        [Fact]
        public void HasProductController()
        {
            var controller = GetFakeProductController();
            Assert.NotNull(controller);
        }

        [Fact]
        public async void ProductControllerReturnProduct()
        {
            ProductController controller = GetFakeProductController();
            var viewResult = (await controller.Index() as ViewResult)?.ViewData.Model as PagedList<Product>;
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void ProductControllerHasViewBagCategory()
        {
            ProductController controller = GetFakeProductController();
            var result = controller.Index();
            var category = controller.ViewBag.Category as IQueryable<ProductCategory>;
            Assert.NotNull(category);
        }
    }
}
