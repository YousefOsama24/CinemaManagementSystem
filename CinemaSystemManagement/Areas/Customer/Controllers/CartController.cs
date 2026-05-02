using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Get cart
        public IActionResult Index()
        {
            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();

            return View(cart);
        }

        // Add movie to cart
        public IActionResult Add(int id)
        {
            var product = _context.Products.Find(id);

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart();
                _context.Carts.Add(cart);
            }

            cart.Items.Add(new CartItem
            {
                ProductId = id,
                Quantity = 1,
                Price = product.Price
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Remove item
        public IActionResult Remove(int id)
        {
            var item = _context.CartItems.Find(id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }

}
