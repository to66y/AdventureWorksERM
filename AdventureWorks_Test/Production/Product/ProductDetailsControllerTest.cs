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
using AdventureWorksERM.Models.Identity;
using Microsoft.AspNetCore.Identity;
using AdventureWorksERM_Test.Production;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using static System.Net.WebRequestMethods;

namespace AdventureWorksERM_Test
{
    public class ProductDetailsControllerTest
    {
        ProductDetailsController controller = new(context, null, null);


        static AdventureWorksContext context = ContextsTest.InitContexts().DBContext;
        //static IdentityContext usersContext = ContextsTest.InitContexts().UsersContext;

        [Fact]
        public void HasDetailsController()
        {
            Assert.NotNull(controller);
        }
        [Fact]
        public async void ReturnsUsersView()
        {
            var result = (await controller.Index(id: 1) as ViewResult)?.ViewName;
            Assert.Equal("UserDetails", result);
        }

        [Fact]
        public void ReturnsAdminView()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var claims = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "Admin"), //Login
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "admin@example.com"),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"), //Role
            };

            controller.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));

            var result = controller.Index(id: 1).Result as ViewResult;
            Assert.Equal("AdminDetails", result.ViewName);
        }

    }
}
