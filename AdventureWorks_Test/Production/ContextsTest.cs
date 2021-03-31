using AdventureWorksERM.Models.DbContexts;
using AdventureWorksERM.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorksERM_Test.Production
{
    public static class ContextsTest
    {
        private static AdventureWorksContext InitTestDB()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseInMemoryDatabase(databaseName: "AWListDatabase" + DateTime.Now.ToFileTimeUtc())
                .Options;

            var context = new AdventureWorksContext(options);
            context.Products.Add(new Product { ProductId = 1, Name = "Prod1", Class = "Class1", ProductSubcategoryId = 1, ListPrice = 2, StandardCost = 3, });
            context.Products.Add(new Product { ProductId = 2, Name = "Prod2", Class = "Class2", ProductSubcategoryId = 2, ListPrice = 4, StandardCost = 2, });
            context.Products.Add(new Product { ProductId = 3, Name = "Prod3", Class = "Class3", ProductSubcategoryId = 3, ListPrice = 3, StandardCost = 1, });

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

        private static IdentityContext InitTestIdentityDB()
        {
            var options = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: "IdentityTestDatabase" + DateTime.Now.ToFileTimeUtc())
                .Options;

            var context = new IdentityContext(options);
            context.Users.Add(new awUser { UserName = "User", Email = "user@example.com", Id = "001" });
            context.Users.Add(new awUser { UserName = "Admin", Email = "admin@example.com", Id = "002" });

            context.Roles.Add(new IdentityRole { Id = "1", Name = "Admin" });

            context.SaveChanges();

            return context;
        }

        public static (AdventureWorksContext DBContext, IdentityContext UsersContext) InitContexts()
        {
            (AdventureWorksContext, IdentityContext) Contexts = (InitTestDB(), InitTestIdentityDB());
            return Contexts;
        }



    }
}
