using CinemaSystemManagement.Data;
using CinemaSystemManagement.Areas.Customer.Models.ViewModels;
using CinemaSystemManagement.Areas.Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .ToList();

            return View(movies);
        }

        // CREATE 
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new MovieVM
            {
                Categories = _context.Categories.ToList(),
                Actors = _context.Actors.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            if (vm.Movie == null) return View(vm);

            var movie = new Movie
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

            _context.Movies.Add(movie);
            _context.SaveChanges();

            // Actors
            if (actorIds != null)
            {
                foreach (var id in actorIds)
                {
                    _context.MovieActors.Add(new MovieActor
                    {
                        MovieId = movie.Id,
                        ActorId = id
                    });
                }
            }

            // Sub Images
            if (SubImgs != null)
            {
                foreach (var img in SubImgs)
                {
                    if (img.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies/sub", fileName);

                        using (var stream = System.IO.File.Create(path))
                        {
                            img.CopyTo(stream);
                        }

                        _context.MovieImages.Add(new MovieImage
                        {
                            MovieId = movie.Id,
                            ImageUrl = fileName
                        });
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // EDIT 
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return NotFound();

            var vm = new MovieVM
            {
                Movie = movie,
                Categories = _context.Categories.ToList(),
                Actors = _context.Actors.ToList(),
                SelectedActors = movie.MovieActors.Select(a => a.ActorId).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            var movie = _context.Movies
                .Include(m => m.MovieActors)
                .Include(m => m.SubImages)
                .FirstOrDefault(m => m.Id == vm.Movie.Id);

            if (movie == null) return NotFound();

            // Update Data
            movie.Name = vm.Movie.Name;
            movie.Description = vm.Movie.Description;
            movie.Price = vm.Movie.Price;
            movie.Status = vm.Movie.Status;
            movie.CategoryId = vm.Movie.CategoryId;

            // Main Img
            if (MainImg != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies", fileName);

                using (var stream = System.IO.File.Create(path))
                {
                    MainImg.CopyTo(stream);
                }

                movie.MainImg = fileName;
            }

            // Update Actors
            _context.MovieActors.RemoveRange(movie.MovieActors);

            if (actorIds != null)
            {
                foreach (var id in actorIds)
                {
                    _context.MovieActors.Add(new MovieActor
                    {
                        MovieId = movie.Id,
                        ActorId = id
                    });
                }
            }

            // Replace SubImages
            if (SubImgs != null && SubImgs.Count > 0)
            {
                _context.MovieImages.RemoveRange(movie.SubImages);

                foreach (var img in SubImgs)
                {
                    if (img.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies/sub", fileName);

                        using (var stream = System.IO.File.Create(path))
                        {
                            img.CopyTo(stream);
                        }

                        _context.MovieImages.Add(new MovieImage
                        {
                            MovieId = movie.Id,
                            ImageUrl = fileName
                        });
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE 
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}