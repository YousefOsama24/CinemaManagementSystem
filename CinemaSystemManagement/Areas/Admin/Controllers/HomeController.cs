using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;
using CinemaSystemManagement.Services;
using CinemaSystemManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class HomeController : Controller
    {
        private readonly IMovieRepo repo;
        private readonly AppDbContext _db;

        public HomeController(IMovieRepo _repo, AppDbContext db)
        {
            repo = _repo;
            _db = db;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            return View();
        }

        // ================= ADMIN MOVIES =================
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
        public async Task<IActionResult> Create(MovieVM vm, IFormFile? MainImg, List<IFormFile>? SubImgs)
        {
            ModelState.Remove("MainImg");
            ModelState.Remove("SubImgs");

            if (!ModelState.IsValid)
            {
                vm.Categories = repo.GetCategories();
                vm.Actors = repo.GetActors();
                return View(vm);
            }

            // ================= SAVE MOVIE =================
            var movie = new Products
            {
                Name = vm.Movie.Name,
                Description = vm.Movie.Description,
                Price = vm.Movie.Price,
                Status = vm.Movie.Status,
                CategoryId = vm.Movie.CategoryId,
                DateTime = DateTime.Now
            };

            // Upload Main Image
            if (MainImg != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine("wwwroot/images/movies", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await MainImg.CopyToAsync(stream);

                movie.MainImg = fileName;
            }

            _db.Products.Add(movie);
            await _db.SaveChangesAsync();

            // ================= ACTORS =================
            if (vm.SelectedActors != null)
            {
                foreach (var actorId in vm.SelectedActors)
                {
                    _db.MovieActors.Add(new MovieActor
                    {
                        MovieId = movie.ProductsId,
                        ActorId = actorId
                    });
                }
            }

            // ================= SUB IMAGES =================
            if (SubImgs != null)
            {
                foreach (var img in SubImgs)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var path = Path.Combine("wwwroot/images/movies/sub", fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await img.CopyToAsync(stream);

                    _db.MovieImages.Add(new MovieImage
                    {
                        MovieId = movie.ProductsId,
                        ImageUrl = fileName
                    });
                }
            }

            // ================= SHOWTIME =================
            var show = new ShowTime
            {
                MovieId = movie.ProductsId,
                StartTime = vm.StartTime,
                Price = vm.ShowPrice
            };

            _db.ShowTimes.Add(show);

            await _db.SaveChangesAsync();

            TempData["Success"] = "Movie created successfully!";
            return RedirectToAction("AdminMovie");
        }

        // ================= EDIT =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = repo.GetById(id);

            if (movie == null) return NotFound();

            var show = _db.ShowTimes.FirstOrDefault(s => s.MovieId == id);

            var vm = new MovieVM
            {
                Movie = movie,
                Categories = repo.GetCategories(),
                Actors = repo.GetActors(),
                SelectedActors = movie.MovieActors.Select(x => x.ActorId).ToList(),
                StartTime = show?.StartTime ?? DateTime.Now,
                ShowPrice = show?.Price ?? movie.Price
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieVM vm, IFormFile? MainImg, List<IFormFile>? SubImgs)
        {
            ModelState.Remove("MainImg");
            ModelState.Remove("SubImgs");

            if (!ModelState.IsValid)
            {
                vm.Categories = repo.GetCategories();
                vm.Actors = repo.GetActors();
                return View(vm);
            }

            var movie = await _db.Products
                .Include(m => m.MovieActors)
                .Include(m => m.SubImages)
                .FirstOrDefaultAsync(m => m.ProductsId == vm.Movie.ProductsId);

            if (movie == null) return NotFound();

            // ================= UPDATE BASIC =================
            movie.Name = vm.Movie.Name;
            movie.Description = vm.Movie.Description;
            movie.Price = vm.Movie.Price;
            movie.Status = vm.Movie.Status;
            movie.CategoryId = vm.Movie.CategoryId;

            // ================= UPDATE MAIN IMAGE =================
            if (MainImg != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine("wwwroot/images/movies", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await MainImg.CopyToAsync(stream);

                movie.MainImg = fileName;
            }

            // ================= UPDATE ACTORS =================
            _db.MovieActors.RemoveRange(movie.MovieActors);

            if (vm.SelectedActors != null)
            {
                foreach (var actorId in vm.SelectedActors)
                {
                    _db.MovieActors.Add(new MovieActor
                    {
                        MovieId = movie.ProductsId,
                        ActorId = actorId
                    });
                }
            }

            // ================= ADD NEW SUB IMAGES =================
            if (SubImgs != null)
            {
                foreach (var img in SubImgs)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var path = Path.Combine("wwwroot/images/movies/sub", fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await img.CopyToAsync(stream);

                    _db.MovieImages.Add(new MovieImage
                    {
                        MovieId = movie.ProductsId,
                        ImageUrl = fileName
                    });
                }
            }

            // ================= UPDATE SHOWTIME =================
            var show = await _db.ShowTimes.FirstOrDefaultAsync(s => s.MovieId == movie.ProductsId);

            if (show != null)
            {
                show.StartTime = vm.StartTime;
                show.Price = vm.ShowPrice;
            }

            await _db.SaveChangesAsync();

            TempData["Success"] = "Movie updated successfully!";
            return RedirectToAction("AdminMovie");
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            var movie = _db.Products.FirstOrDefault(x => x.ProductsId == id);

            if (movie == null)
            {
                TempData["Error"] = "Movie not found!";
                return RedirectToAction("AdminMovie");
            }

            _db.Products.Remove(movie);
            _db.SaveChanges();

            TempData["Success"] = "Movie deleted successfully!";
            return RedirectToAction("AdminMovie");
        }
    }
}