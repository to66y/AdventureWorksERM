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

namespace AdventureWorksCRM_1_0_Test
{
    public class ProductControllerTest
    {
        public class FakeProductsRepository : IRepository<Product>
        {
            public IQueryable<Product> Storage => _products;
            IQueryable<Product> _products = new List<Product>
            {
                new Product(){ Name="Name1", Class="Class1", Color="Color1" },
                new Product(){ Name="Name2", Class="Class2", Color="Color2"  },
                new Product(){ Name="Name3", Class="Class3", Color="Color3"  },
                new Product(){ Name="Name4", Class="Class4", Color="Color4"  },

            }.AsQueryable();

            public void Add(Product p) { }
        }

        public class FakeCategoriesRepository: IRepository<ProductCategory>
        {
            public IQueryable<ProductCategory> Storage => _categories;
            IQueryable<ProductCategory> _categories = new List<ProductCategory>
            {
                new ProductCategory(){ Name="Cat1", },
                new ProductCategory(){ Name="Cat2", },
                new ProductCategory(){ Name="Cat3", },
                new ProductCategory(){ Name="Cat4", },

            }.AsQueryable<ProductCategory>();

            public void Add(ProductCategory p) { }
        }
        private ProductController ReturnFakeProductController()
        {
            string[] categoryNames = new string[] { "Cat1", "Cat2", "Cat3" };
            var mockProductRepo = new Mock<IRepository<Product>>();
            //var mockCategoryRepo = new Mock<IRepository<ProductCategory>>();
            //var mockProductCats = new Mock<IEnumerable<ProductCategory>>();
            //mockProductRepo.SetupGet(p => p.Storage).Returns(ProductsRepository);
            //mockCategoryRepo.SetupGet(pc => pc.Storage).Returns(mockProductCats.Object);
            return new ProductController(ProductsRepository, CategoriesRepository);
        }

        public IRepository<Product> ProductsRepository { get => new FakeProductsRepository(); }
        public IRepository<ProductCategory> CategoriesRepository{ get => new FakeCategoriesRepository(); }



        [Fact]
        public void HasProductController()
        {
            var controller = ReturnFakeProductController();
            Assert.NotNull(controller);
        }

        [Fact]
        //[ClassData(typeof(ProductTestData))]
        public void ProductControllerReturnProduct()
        {
            ProductController controller = ReturnFakeProductController();
            var result = (controller.Index().Result as ViewResult)?.ViewData.Model as PagedList<Product>;
            Assert.Equal(controller.ProductRepository.Storage, result);
        }

        [Fact]
        public void ProductControllerHasViewBagCategory()
        {
            ProductController controller = ReturnFakeProductController();
            var result = controller.Index().Result;
            var category = controller.ViewBag.Category;
            Assert.Equal("Cat1", (category as IQueryable<ProductCategory>).First().Name);
        }
    }
}
