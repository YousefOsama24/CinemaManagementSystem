using CinemaSystemManagement.Models;
using CinemaSystemManagement.Models.ViewModels;

namespace CinemaSystemManagement.Services
{
    public interface IMovieRepo
    {
        List<Products> GetAll();
        Products GetById(int id);

        void Add(Products movie);

        void AddActors(int movieId, List<int> actorIds);
        void AddSubImages(int movieId, List<IFormFile> images);

        void Update(MovieVM vm, IFormFile MainImg, List<IFormFile> SubImgs, List<int> actorIds);

        bool Delete(int id);

        List<Category> GetCategories();
        List<Actor> GetActors();
    }
}