using AdventureWorksCRM_1_0.Controllers;
using AdventureWorksCRM_1_0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AdventureWorksCRM_1_0.Models.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksCRM_1_0.Infrastructure;
using AdventureWorksCRM_1_0.Models.Helpers;

namespace AdventureWorksCRM_1_0_Test
{
    public class ProductControllerTest
    {
        public class FakeProductsRepository : IProductRepository
        {
            public IEnumerable<Product> Products => _products;
            Product[] _products = new Product[]
            {
                new Product(){ Name="Name1", Class="Class1", Color="Color1" },
                new Product(){ Name="Name2", Class="Class2", Color="Color2"  },
                new Product(){ Name="Name3", Class="Class3", Color="Color3"  },
                new Product(){ Name="Name4", Class="Class4", Color="Color4"  },

            };

            public void AddProduct(Product p) { }
        }

        public IProductRepository ProductsRepository { get => new FakeProductsRepository(); }



        [Fact]
        public void HasProductController()
        {
            var controller = new ProductController(ProductsRepository);
            Assert.NotNull(controller);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void ProductControllerReturnProduct(Product[] products)
        {
            var mock = new Mock<IProductRepository>();
            mock.SetupGet(p => p.Products).Returns(products);

            var controller = new ProductController(ProductsRepository);
            var result = (controller.Index().Result as ViewResult)?.ViewData.Model as PagedList<Product>;
            Assert.Equal(controller.Repository.Products, result);
        }

    }
}
