using CinemaSystemManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CinemaSystemManagement.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieImage> MovieImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Seat> Seats { get; set; }





    }
}
