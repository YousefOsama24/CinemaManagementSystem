using CinemaSystemManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystemManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SeatController : Controller
    {
        private readonly AppDbContext _context;

        public SeatController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int movieId)
        {
            var seats = _context.Seats
                .Where(s => s.MovieId == movieId)
                .ToList();

            return View(seats);
        }

        [HttpPost]
        public IActionResult Select(string selectedSeats)
        {
            TempData["Seats"] = selectedSeats;
            return RedirectToAction("Index", "Checkout");
        }
    }
}
