using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;
using CinemaSystemManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class ShowTimeController : Controller
    {
        private readonly AppDbContext _db;

        public ShowTimeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var shows = _db.ShowTimes.Include(s => s.Movie).ToList();
            return View(shows);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieVM vm, DateTime StartTime, decimal ShowPrice)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // ================= SAVE MOVIE =================
            var movie = new Products
            {
                Name = vm.Movie.Name,
                Description = vm.Movie.Description,
                Price = vm.Movie.Price,
                Status = vm.Movie.Status,
                CategoryId = vm.Movie.CategoryId,
                MainImg = vm.Movie.MainImg
                
            };

            _db.Products.Add(movie);
            await _db.SaveChangesAsync(); 

            // ================= SAVE SHOWTIME =================
            var show = new ShowTime
            {
                MovieId = movie.ProductsId, 
                StartTime = StartTime,
                Price = ShowPrice
            };

            _db.ShowTimes.Add(show);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create(ShowTime model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _db.ShowTimes.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var show = _db.ShowTimes.Find(id);
            ViewBag.Movies = _db.Products.ToList();
            return View(show);
        }

        [HttpPost]
        public IActionResult Edit(ShowTime model)
        {
            _db.ShowTimes.Update(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var show = _db.ShowTimes.Find(id);
            _db.ShowTimes.Remove(show);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
