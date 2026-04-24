using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;
using CinemaSystemManagement.Utility;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Services
{
    public class MovieRepo : IMovieRepo
    {
        private readonly AppDbContext _context;
        private readonly FileService _fileService;

        public MovieRepo(AppDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public List<Products> GetAll()
        {
            return _context.Products
                .Include(x => x.Category)
                .ToList();
        }

        public Products GetById(int id)
        {
            return _context.Products
                .Include(x => x.Category)
                .Include(x => x.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .Include(x => x.SubImages)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Add(Products movie)
        {
            _context.Products.Add(movie);
            _context.SaveChanges();
        }

        public void AddActors(int movieId, List<int> actorIds)
        {
            foreach (var id in actorIds)
            {
                _context.MovieActors.Add(new MovieActor
                {
                    MovieId = movieId,
                    ActorId = id
                });
            }

            _context.SaveChanges();
        }

        public void AddSubImages(int movieId, List<IFormFile> images)
        {
            foreach (var file in images)
            {
                var fileName = _fileService.Upload(file, ImgType.Sub);

                _context.MovieImages.Add(new MovieImage
                {
                    MovieId = movieId,
                    ImageUrl = fileName
                });
            }

            _context.SaveChanges();
        }

        public void Update(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            var movie = GetById(vm.Movie.Id);

            movie.Name = vm.Movie.Name;
            movie.Description = vm.Movie.Description;
            movie.Price = vm.Movie.Price;
            movie.Status = vm.Movie.Status;
            movie.CategoryId = vm.Movie.CategoryId;

            if (MainImg != null)
            {
                if (!string.IsNullOrEmpty(movie.MainImg))
                    _fileService.Delete(movie.MainImg, ImgType.Main);

                movie.MainImg = _fileService.Upload(MainImg, ImgType.Main);
            }

            // Update actors
            var oldActors = _context.MovieActors.Where(x => x.MovieId == movie.Id);
            _context.MovieActors.RemoveRange(oldActors);

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

            // Add sub images
            if (SubImgs != null)
            {
                AddSubImages(movie.Id, SubImgs);
            }

            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var movie = GetById(id);
            if (movie == null) return false;

            _context.Products.Remove(movie);
            _context.SaveChanges();
            return true;
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public List<Actor> GetActors()
        {
            return _context.Actors.ToList();
        }
    }
}