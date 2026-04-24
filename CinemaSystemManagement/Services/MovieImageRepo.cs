using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaSystemManagement.Services
{
    public class MovieImageRepo : IMovieImageRepo
    {
        private readonly AppDbContext db;

        public MovieImageRepo(AppDbContext db)
        {
            this.db = db;
        }

        public async Task CreateAsync(MovieImage img)
        {
            await db.MovieImages.AddAsync(img);
        }

        public async Task<List<MovieImage>> GetByMovieId(int id)
        {
            return await db.MovieImages
                .Where(x => x.MovieId == id)
                .ToListAsync();
        }

        public void DeleteRange(List<MovieImage> imgs)
        {
            db.MovieImages.RemoveRange(imgs);
        }
    }
}
