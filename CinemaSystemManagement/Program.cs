using CinemaSystem.Data;
using CinemaSystem.Services;

namespace CinemaSystemManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<FileService>();
            builder.Services.AddDbContext<AppDbContext>();

            var app = builder.Build();

            // Middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
