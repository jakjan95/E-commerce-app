using E_commerce_app.Data;
using E_commerce_app.Data.Models;
using E_commerce_app.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Controllers
{
    //dodanie tego pod funkcjami blokuje dostep uzytkownikowi
    //[Authorize(Policy = "RequireAdministratorRole")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? categoryId)
        {
            if (categoryId == null)
                return View(await _context.Products.ToListAsync());

            return View(await _context.Products
                .Include(a => a.Categories)
                .Where(a => a.Categories.Any(b => b.CategoryId == categoryId))
                .ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = GetProductToDetailsPage(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var result = new ProductCreateDto
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(result);
        }

        //tak samo delet review :)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(int id, int rating, string comment)
        {
            var product = _context.Products
                .Include(a=>a.Reviews)
                .ThenInclude(a=>a.User)
                .FirstOrDefault(a => a.Id == id);
            if (product == null)
                return View("Index");
            var user = await _userManager.GetUserAsync(HttpContext.User);
            product.Reviews.Add(new Review
            {
                Comment = comment,
                Date = DateTime.Now,
                Rating = rating,
                User = user
            });
            _context.SaveChanges();


            return View("Details",product);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Image")] ProductCreateDto product, string Price, List<int> Categories)
        {
            if (Decimal.TryParse(Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                product.Price = result;
            }
            else
            {
                return View(product);
            }
            var productToAdd = new Product
            {
                Image = product.Image,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Categories = Categories.Select(a => new ProductCategory { CategoryId = a }).ToList()
            };

            if (ModelState.IsValid)
            {
                _context.Add(productToAdd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(a => a.Categories)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
            var categories = await _context.Categories.ToListAsync();
            if (product == null)
            {
                return NotFound();
            }
            var productToView = new ProductCreateDto
            {
                Name = product.Name,
                Categories = categories,
                SelectedCategories = product.Categories.Select(a => a.Category).ToList(),
                Description = product.Description,
                Id = product.Id,
                Image = product.Image,
                Price = product.Price
            };
            return View(productToView);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image")] ProductCreateDto product, string Price, List<int> Categories)
        {
            if (Decimal.TryParse(Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                product.Price = result;
            }
            else
            {
                return View(product);
            }

            if (id != product.Id)
            {
                return NotFound();
            }

            var productFromDatabase = _context.Products.Include(a => a.Categories).FirstOrDefault(a => a.Id == product.Id);

            List<ProductCategory> listToRemove = new List<ProductCategory>();
            List<ProductCategory> listToAdd = new List<ProductCategory>();
            foreach (var item in productFromDatabase.Categories)
            {
                if (!Categories.Contains(item.CategoryId))
                    listToRemove.Add(item);
            }
            foreach (var item in Categories)
            {
                if (!productFromDatabase.Categories.Any(a => a.Id == item))
                    listToAdd.Add(new ProductCategory { CategoryId = item });
            }
            foreach (var item in listToRemove)
            {
                productFromDatabase.Categories.Remove(item);
            }

            productFromDatabase.Image = product.Image;
            productFromDatabase.Name = product.Name;
            productFromDatabase.Description = product.Description;
            productFromDatabase.Price = product.Price;
            productFromDatabase.Categories.AddRange(listToAdd);

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: Products/Delete/5
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
            return _context.Products.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int? id, int? productId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(review);
            _context.SaveChanges();

            var product = GetProductToDetailsPage(productId??0);
            return View("Details", product);
        }

        private Product GetProductToDetailsPage(int id)
        {
            return _context.Products
                .Include(a => a.Reviews)
                .ThenInclude(a => a.User)
                .FirstOrDefault(m => m.Id == id);
        }
    }
}