using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationdbContext _context;

        public CartController(ApplicationdbContext context)
        {
            _context = context;
        }

        // read  
        public async Task<IActionResult> Index()
        {
            var items = await _context.CartItems.ToListAsync();
            ViewBag.Total = items.Sum(i => i.Price * i.Quantity);
            return View(items);
        }

        // create product to cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.productId == productId);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItems
                {
                    productId = product.Id,
                    productName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // update quantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null && quantity >= 1)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // delete function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}