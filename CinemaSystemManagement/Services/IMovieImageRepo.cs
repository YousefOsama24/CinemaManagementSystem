using CinemaSystemManagement.Models;

namespace CinemaSystemManagement.Services
{
    public interface IMovieImageRepo
    {
        Task CreateAsync(MovieImage img);
        Task<List<MovieImage>> GetByMovieId(int id);
        void DeleteRange(List<MovieImage> imgs);
    }
}
