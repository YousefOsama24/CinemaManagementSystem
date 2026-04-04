using CinemaSystem.Data;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db = new();

        public IActionResult Index(int page = 1, string search = "")
        {
            int pageSize = 3;

            var moviesQuery = db.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(search));
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

        public IActionResult Categories()
        {
            var categories = db.Categories
                .Include(c => c.Movies)
                .ToList();

            return View(categories);
        }

        public IActionResult CategoryMovies(int id)
        {
            var movies = db.Movies.Where(m => m.CategoryId == id).ToList();

            ViewBag.CategoryName = db.Categories.FirstOrDefault(c => c.Id == id)?.CategoryName;

            return View(movies);
        }

        public IActionResult Transactions()
        {
            return View(db.Transactions.ToList());
        }

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