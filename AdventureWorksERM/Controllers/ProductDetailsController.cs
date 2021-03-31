using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdventureWorksERM.Models.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AdventureWorksERM.Models.Identity;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorksERM.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly AdventureWorksContext _context;
        private readonly UserManager<awUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProductDetailsController(AdventureWorksContext context, UserManager<awUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //ToDo: comment below and folow ~/ProductDetails/Grant/ to get Admin role
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Grant()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = new IdentityRole("Admin");
            await _roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok();
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductModel)
                .Include(p => p.ProductSubcategory)
                .Include(p => p.ProductReviews)
                .Include(p => p.ProductProductPhotos)
                .ThenInclude(x => x.ProductPhoto)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            if (User!=null && User.IsInRole("Admin"))
            {
                return View("AdminDetails", product);
            }
            else
            {
                return View("UserDetails", product);
            }
        }

        [HttpPost]
        public IActionResult DeleteComment(int id)
        {
            var review = _context.ProductReviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            if (User == null || !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.ProductReviews.Remove(review);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int id, DateTime date, string user, int rating, string comment)
        {
            if (String.IsNullOrEmpty(user))
            {
                return LocalRedirect("~/Identity/Account/Login");
            }
            if (String.IsNullOrEmpty(comment))
            {
                return RedirectToAction("Index", new { id });
            }

            var awuser = await _userManager.FindByIdAsync(user);
            var review = new ProductReview()
            {
                ProductReviewId = 0,
                ProductId = id,
                ReviewDate = date,
                EmailAddress = awuser.Email,
                Rating = rating,
                Comments = comment,
                ReviewerName = awuser.UserName,
            };

            _context.ProductReviews.Add(review);
            _context.SaveChanges();
            return RedirectToAction("Index", new { id });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "Name");
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "Name");
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode");
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductNumber,MakeFlag,FinishedGoodsFlag,Color,SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,Size,SizeUnitMeasureCode,WeightUnitMeasureCode,Weight,DaysToManufacture,ProductLine,Class,Style,ProductSubcategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,Rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "Name", product.ProductModelId);
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "Name", product.ProductSubcategoryId);
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.SizeUnitMeasureCode);
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.WeightUnitMeasureCode);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "Name", product.ProductModelId);
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "Name", product.ProductSubcategoryId);
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.SizeUnitMeasureCode);
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.WeightUnitMeasureCode);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductNumber,MakeFlag,FinishedGoodsFlag,Color,SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,Size,SizeUnitMeasureCode,WeightUnitMeasureCode,Weight,DaysToManufacture,ProductLine,Class,Style,ProductSubcategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,Rowguid,ModifiedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = id });
            }
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "Name", product.ProductModelId);
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "Name", product.ProductSubcategoryId);
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.SizeUnitMeasureCode);
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.WeightUnitMeasureCode);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductModel)
                .Include(p => p.ProductSubcategory)
                .Include(p => p.SizeUnitMeasureCodeNavigation)
                .Include(p => p.WeightUnitMeasureCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
