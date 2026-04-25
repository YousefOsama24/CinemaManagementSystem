using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;
using CinemaSystemManagement.Services;
using CinemaSystemManagement.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystemManagement.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class HomeController : Controller
    {
        private readonly IMovieRepo repo;

        public HomeController(IMovieRepo _repo)
        {
            repo = _repo;
        }

        // ================= INDEX =================

        public IActionResult Index()
        {
            return View();
        }
        // ================= AdminMovie =================

        public IActionResult AdminMovie()
        {
            var movies = repo.GetAll();
            return View(movies);
        }

        // ================= CREATE =================

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new MovieVM
            {
                Categories = repo.GetCategories(),
                Actors = repo.GetActors()
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult Create(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors";

                vm.Categories = repo.GetCategories();
                vm.Actors = repo.GetActors();

                return View(vm);
            }

            var movie = new Products
            {
                Name = vm.Movie.Name,
                Description = vm.Movie.Description,
                Price = vm.Movie.Price,
                Status = vm.Movie.Status,
                CategoryId = vm.Movie.CategoryId
            };

            // Main Image
            if (MainImg != null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies", fileName);

                using (var stream = System.IO.File.Create(path))
                {
                    MainImg.CopyTo(stream);
                }

                movie.MainImg = fileName;
            }

            repo.Add(movie);

            // Actors
            if (actorIds != null)
            {
                repo.AddActors(movie.Id, actorIds);
            }

            // Sub Images
            if (SubImgs != null)
            {
                repo.AddSubImages(movie.Id, SubImgs);
            }

            TempData["Success"] = "Movie added successfully!";
            return RedirectToAction("Index");
        }

        // ================= EDIT =================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = repo.GetById(id);

            if (movie == null) return NotFound();

            var vm = new MovieVM
            {
                Movie = movie,
                Categories = repo.GetCategories(),
                Actors = repo.GetActors()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors";

                vm.Categories = repo.GetCategories();
                vm.Actors = repo.GetActors();

                return View(vm);
            }

            repo.Update(vm, MainImg, SubImgs, actorIds);

            TempData["Success"] = "Movie updated successfully!";
            return RedirectToAction("Index");
        }

        // ================= DELETE =================

        public IActionResult Delete(int id)
        {
            bool deleted = repo.Delete(id);

            if (deleted)
                TempData["Success"] = "Movie deleted successfully!";
            else
                TempData["Error"] = "Movie not found!";

            return RedirectToAction("Index");
        }
    }
}