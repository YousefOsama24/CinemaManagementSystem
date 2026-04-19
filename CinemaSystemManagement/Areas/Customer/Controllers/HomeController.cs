using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly AppDbContext db;

        public HomeController(AppDbContext context)
        {
            db = context;
        }

        // ================== INDEX ==================
        public IActionResult Index()
        {
            return View();
        }

        // ================== MOVIES (FIXED) ==================
        public IActionResult Movie(int page = 1, string search = "")
        {
            if (page < 1) page = 1;

            int pageSize = 3;

            var moviesQuery = db.Movies
                .Include(m => m.Category)
                .AsQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                moviesQuery = moviesQuery
                    .Where(m => m.Name.Contains(search));
            }

            int totalMovies = moviesQuery.Count();

            var movies = moviesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalMovies / pageSize);
            ViewBag.Search = search;

            return View(movies);
        }

        // ================== DETAILS ==================
        public IActionResult Details(int id)
        {
            var movie = db.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.SubImages)
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // ================== CATEGORIES ==================
        public IActionResult Categories()
        {
            var categories = db.Categories
                .Include(c => c.Movies)
                .ToList();

            return View(categories);
        }

        // ================== CATEGORY MOVIES ==================
        public IActionResult CategoryMovies(int id)
        {
            var category = db.Categories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            var movies = db.Movies
                .Where(m => m.CategoryId == id)
                .ToList();

            ViewBag.CategoryName = category.CategoryName;

            return View(movies);
        }

        // ================== TRANSACTIONS ==================
        public IActionResult Transactions()
        {
            var transactions = db.Transactions.ToList();
            return View(transactions);
        }

        // ================== ADD TRANSACTION (SAFE) ==================
        [HttpPost]
        public IActionResult AddTransaction(int id)
        {
            var movie = db.Movies.Find(id);

            if (movie == null)
                return NotFound();

            var t = new Transaction
            {
                MovieName = movie.Name,
                MovieImg = movie.MainImg,
                Price = movie.Price,
                Date = DateTime.Now
            };

            db.Transactions.Add(t);
            db.SaveChanges();

            return RedirectToAction("Transactions");
        }

        // ================== DELETE TRANSACTION (SAFE) ==================
        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            var t = db.Transactions.Find(id);

            if (t == null)
                return NotFound();

            db.Transactions.Remove(t);
            db.SaveChanges();

            return RedirectToAction("Transactions");
        }
    }
}