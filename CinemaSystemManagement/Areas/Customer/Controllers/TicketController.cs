using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystemManagement.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class TicketController : Controller
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tickets = _context.Tickets.ToList();
            return View(tickets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ticket ticket)
        {
            if (!ModelState.IsValid)
                return View(ticket);

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
