using AdventureWorksERM.Controllers;
using AdventureWorksERM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AdventureWorksERM.Models.DbContexts;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksERM.Models.Helpers;
using Microsoft.EntityFrameworkCore;
using AdventureWorksERM.Models.Production.ViewModels;
using AdventureWorksERM_Test.Production;

namespace AdventureWorksERM_Test
{
    public class ProductControllerTest
    {
        static AdventureWorksContext context = ContextsTest.InitContexts().DBContext;
        ProductController controller = new(context);

        [Fact]
        public void HasProductController()
        {
            Assert.NotNull(controller);
        }

        [Fact]
        public async void ReturnSomething()
        {
            var viewResult = (await controller.Index(orderby: null, category: null) as ViewResult)?.ViewData.Model;
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async void NullCategory()
        {
            int? category = null;
            var result = (await controller.Index(orderby: null, category) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.NotEmpty(result.Products);
        }

        [Fact]
        public async void FilterByCategory()
        {
            int? category = 1;
            var result = (await controller.Index(orderby: null, category) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod1", result.Products.First().Name);
        }

        [Fact]
        public async void OrderByPriceDesc()
        {
            var result = (await controller.Index("price_desc", category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod2", result.Products.First().Name); 
        }

        [Fact]
        public async void OrderByPriceAsc()
        {
            var result = (await controller.Index("Price", category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod1", result.Products.First().Name);
        }

        [Fact]
        public async void OrderByNameDesc()
        {
            var result = (await controller.Index("name_desc", category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod3", result.Products.First().Name);
        }

        [Fact]
        public async void OrderByHasValue()
        {
            await controller.Index("price_desc", category: null);
            Assert.NotNull(controller.ViewData["PriceSortParm"]);
            await controller.Index(orderby: null, category: null);
            Assert.Equal("name_desc", controller.ViewData["NameSortParm"]);
        }

        [Fact]
        public async void SearchTest()
        {
            var searchName = "Prod3";
            var result = (await controller.Index(search: searchName, orderby: null, category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod3", result.Products.First().Name);
        }

        [Fact]
        public async void HasPhoto()
        {
            var searchName = "Prod1";
            var result = (await controller.Index(search: searchName, orderby: null, category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal(new byte[] { 1, 2, 3, }, result.ProductPhoto.First().ThumbNailPhoto);
        }
    }
}
