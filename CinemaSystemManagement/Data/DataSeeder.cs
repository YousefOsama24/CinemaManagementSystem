using CinemaSystemManagement.Models;

namespace CinemaSystemManagement.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Products.Any()) return; 

            
            var categories = new List<Category>
            {
                new Category { CategoryName = "Action" },
                new Category { CategoryName = "Drama" },
                new Category { CategoryName = "Sci-Fi" },
                new Category { CategoryName = "Comedy" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            
            var actors = new List<Actor>
            {
                new Actor { Name = "Leonardo DiCaprio", Img = "Leonardo_DiCaprio.jpg" },
                new Actor { Name = "Tom Hardy", Img = "Tom_Hardy.jpeg" },
                new Actor { Name = "Zendaya", Img = "Zendaya.jpg" },
                new Actor { Name = "Keanu Reeves", Img = "Keanu_Reeves.jpg" },
                new Actor { Name = "Anne Hathaway", Img = "Anne_Hathaway.jpg" }
            };

            context.Actors.AddRange(actors);
            context.SaveChanges();

            
            var movies = new List<Products>
            {
                new Products
                {
                    Name = "Inception",
                    Description = "A mind-bending thriller",
                    Price = 100,
                    MainImg = "move1.jpg",
                    CategoryId = categories[2].Id
                },
                new Products
                {
                    Name = "John Wick",
                    Description = "Action revenge movie",
                    Price = 120,
                    MainImg = "move2.jpg",
                    CategoryId = categories[0].Id
                },
                new Products
                {
                    Name = "Dune",
                    Description = "Epic sci-fi adventure",
                    Price = 150,
                    MainImg = "move3.jpg",
                    CategoryId = categories[2].Id
                }
            };

            context.Products.AddRange(movies);
            context.SaveChanges();

            var movieActors = new List<MovieActor>
            {
                new MovieActor { MovieId = movies[0].Id, ActorId = actors[0].Id },
                new MovieActor { MovieId = movies[0].Id, ActorId = actors[1].Id },

                new MovieActor { MovieId = movies[1].Id, ActorId = actors[3].Id },

                new MovieActor { MovieId = movies[2].Id, ActorId = actors[2].Id },
                new MovieActor { MovieId = movies[2].Id, ActorId = actors[4].Id }
            };

            context.MovieActors.AddRange(movieActors);
            context.SaveChanges();

            var images = new List<MovieImage>
            {
                new MovieImage { MovieId  = movies[0].Id, ImageUrl = "move4.jpg" },
                new MovieImage { MovieId  = movies[0].Id, ImageUrl = "move5.jpg" },

                new MovieImage { MovieId  = movies[1].Id, ImageUrl = "move6.jpg" },

                new MovieImage { MovieId  = movies[2].Id, ImageUrl = "move7.jpg" }
            };

            context.MovieImages.AddRange(images);
            context.SaveChanges();
        }
    }
}
