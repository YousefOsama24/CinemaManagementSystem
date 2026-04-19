using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using Microsoft.EntityFrameworkCore;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;

namespace CinemaSystemManagement.Services
{
    public class MovieRepo : IMovieRepo
    {
        private readonly AppDbContext _context;

        public MovieRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.Include(m => m.Category).ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies
                .Include(m => m.MovieActors)
                .Include(m => m.SubImages)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return false;

            _context.Movies.Remove(movie);
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
            foreach (var img in images)
            {
                if (img.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies/sub", fileName);

                    using var stream = System.IO.File.Create(path);
                    img.CopyTo(stream);

                    _context.MovieImages.Add(new MovieImage
                    {
                        MovieId = movieId,
                        ImageUrl = fileName
                    });
                }
            }

            _context.SaveChanges();
        }

        public void Update(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds)
        {
            var movie = _context.Movies
                .Include(m => m.MovieActors)
                .Include(m => m.SubImages)
                .FirstOrDefault(m => m.Id == vm.Movie.Id);

            if (movie == null) return;

            movie.Name = vm.Movie.Name;
            movie.Description = vm.Movie.Description;
            movie.Price = vm.Movie.Price;
            movie.Status = vm.Movie.Status;
            movie.CategoryId = vm.Movie.CategoryId;

            if (MainImg != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies", fileName);

                using var stream = System.IO.File.Create(path);
                MainImg.CopyTo(stream);

                movie.MainImg = fileName;
            }

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

            if (SubImgs != null && SubImgs.Count > 0)
            {
                _context.MovieImages.RemoveRange(movie.SubImages);

                foreach (var img in SubImgs)
                {
                    if (img.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies/sub", fileName);

                        using var stream = System.IO.File.Create(path);
                        img.CopyTo(stream);

                        _context.MovieImages.Add(new MovieImage
                        {
                            MovieId = movie.Id,
                            ImageUrl = fileName
                        });
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}