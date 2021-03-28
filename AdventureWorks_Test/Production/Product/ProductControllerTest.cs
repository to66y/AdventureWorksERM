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

namespace AdventureWorksERM_Test
{
    public class ProductControllerTest
    {
        private static AdventureWorksContext InitTestDB()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseInMemoryDatabase(databaseName: "AWListDatabase" + DateTime.Now.ToFileTimeUtc())
                .Options;

            var context = new AdventureWorksContext(options);
            context.Products.Add(new Product { Name = "Prod1", Class = "Class1", ProductSubcategoryId = 1, ListPrice = 2, StandardCost = 3, });
            context.Products.Add(new Product { Name = "Prod2", Class = "Class2", ProductSubcategoryId = 2, ListPrice = 4, StandardCost = 2, });
            context.Products.Add(new Product { Name = "Prod3", Class = "Class3", ProductSubcategoryId = 3, ListPrice = 3, StandardCost = 1, });

            context.ProductCategories.Add(new ProductCategory { Name = "Cat1", ProductCategoryId = 1, });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat2", ProductCategoryId = 2, });
            context.ProductCategories.Add(new ProductCategory { Name = "Cat3", ProductCategoryId = 3, });

            context.ProductSubcategories.Add(new ProductSubcategory { Name = "SubCat1", ProductSubcategoryId = 1, ProductCategoryId = 1 });
            context.ProductSubcategories.Add(new ProductSubcategory { Name = "SubCat2", ProductSubcategoryId = 2, ProductCategoryId = 2 });
            context.ProductSubcategories.Add(new ProductSubcategory { Name = "SubCat3", ProductSubcategoryId = 3, ProductCategoryId = 3 });

            context.ProductPhotos.Add(new ProductPhoto { ProductPhotoId = 1, ThumbNailPhoto = new byte[] { 1, 2, 3 } });
            context.ProductPhotos.Add(new ProductPhoto { ProductPhotoId = 2, ThumbNailPhoto = new byte[] { 2, 3, 1 } });
            context.ProductPhotos.Add(new ProductPhoto { ProductPhotoId = 3, ThumbNailPhoto = new byte[] { 3, 1, 2 } });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void HasProductController()
        {
            var context = InitTestDB();
            ProductController controller = new(context);
            Assert.NotNull(controller);
        }

        [Fact]
        public async void ReturnSomething()
        {
            var context = InitTestDB();
            ProductController controller = new(context);
            var viewResult = (await controller.Index(orderby: null, category: null) as ViewResult)?.ViewData.Model;
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async void NullCategory()
        {
            var context = InitTestDB();
            ProductController controller = new(context);
            int? category = null;
            var result = (await controller.Index(orderby: null, category) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.NotEmpty(result.Products);
        }

        [Fact]
        public async void FilterByCategory()
        {
            var context = InitTestDB();
            ProductController controller = new(context);
            int? category = 1;
            var result = (await controller.Index(orderby: null, category) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod1", result.Products.First().Name);
        }

        [Fact]
        public async void OrderByCostAsc()
        {
            var context = InitTestDB();
            ProductController controller = new(context);
            var result = (await controller.Index("price_desc", category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod2", result.Products.First().Name); 
        }

        [Fact]
        public async void SearchTest()
        {
            var context = InitTestDB();
            ProductController controller = new(context);
            var searchName = "Prod3";
            var result = (await controller.Index(search: searchName, orderby: null, category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal("Prod3", result.Products.First().Name);
        }

        [Fact]
        public async void HasPhoto()
        {
            var contex = InitTestDB();
            ProductController controller = new(contex);
            var searchName = "Prod1";
            var result = (await controller.Index(search: searchName, orderby: null, category: null) as ViewResult)?.ViewData.Model as ProductsViewModel;
            Assert.Equal(new byte[] { 1, 2, 3, }, result.ProductPhoto.First().ThumbNailPhoto);
        }
    }
}
