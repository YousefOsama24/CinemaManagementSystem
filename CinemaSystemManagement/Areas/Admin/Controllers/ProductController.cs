using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;
using CinemaSystemManagement.Services;
using CinemaSystemManagement.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystemManagement.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class ProductController : Controller
    {
        private readonly IMovieRepo _repo;
        private readonly FileService _fileService;

        public ProductController(IMovieRepo repo, FileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var movies = _repo.GetAll();
            return View(movies);
        }

        // ================= CREATE =================

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new MovieVM
            {
                Categories = _repo.GetCategories(),
                Actors = _repo.GetActors()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _repo.GetCategories();
                vm.Actors = _repo.GetActors();
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
            if (MainImg != null)
                movie.MainImg = _fileService.Upload(MainImg, ImgType.Main);

            _repo.Add(movie);

            // Actors
            if (actorIds != null)
                _repo.AddActors(movie.Id, actorIds);

            // Sub Images
            if (SubImgs != null)
                _repo.AddSubImages(movie.Id, SubImgs);

            return RedirectToAction("Index");
        }

        // ================= EDIT =================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _repo.GetById(id);
            if (movie == null) return NotFound();

            var vm = new MovieVM
            {
                Movie = movie,
                Categories = _repo.GetCategories(),
                Actors = _repo.GetActors(),
                SelectedActors = movie.MovieActors.Select(x => x.ActorId).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _repo.GetCategories();
                vm.Actors = _repo.GetActors();
                return View(vm);
            }

            _repo.Update(vm, MainImg, SubImgs, actorIds);

            return RedirectToAction("Index");
        }

        // ================= DELETE =================

        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}