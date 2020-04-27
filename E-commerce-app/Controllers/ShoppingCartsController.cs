using E_commerce_app.Data;
using E_commerce_app.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShoppingCartsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ShoppingCarts
        [Authorize]
        public async Task<IActionResult> Index(int? id, int? quantity)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var shoppingCart = _context.ShoppingCarts
                .Include(a => a.ShoppingCartProducts)
                .ThenInclude(a => a.Product)
                .FirstOrDefault(a => a.UserId == userId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart { UserId = userId };
                _context.Add(shoppingCart);
                _context.SaveChanges();
            }
            if (id == null)
            {
                return View(shoppingCart.ShoppingCartProducts);
            }

            if (shoppingCart.ShoppingCartProducts.Any(a => a.Product.Id == id))
            {
                shoppingCart.ShoppingCartProducts
                    .First(a => a.Product.Id == id)
                    .Quantity += quantity ?? 1;
                _context.SaveChanges();
                return View(shoppingCart.ShoppingCartProducts);
            }

            var product = _context.Products.FirstOrDefault(a => a.Id == id);
            if (product != null)
            {
                shoppingCart.ShoppingCartProducts.Add(new ShoppingCartProduct { ProductId = product.Id, Quantity = quantity ?? 1 });
                _context.SaveChanges();
            }

            return View(shoppingCart.ShoppingCartProducts);
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
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
            ViewData["UserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //przerobic geta na tego posta nizej
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartProduct = await _context.ShoppingCartProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCartProduct == null)
            {
                return NotFound();
            }

            if (shoppingCartProduct.Quantity <= 1)
            {
                _context.Remove(shoppingCartProduct);
                _context.SaveChanges();
            }
            else
            {
                shoppingCartProduct.Quantity--;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCarts.Any(e => e.Id == id);
        }
    }
}