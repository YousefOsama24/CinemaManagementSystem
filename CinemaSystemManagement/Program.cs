using CinemaSystemManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// 🔥 Add Services
builder.Services.AddControllersWithViews();


// 🔥 Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 🔥 Repository (SOLID)
builder.Services.AddScoped<IMovieRepo, MovieRepo>();


var app = builder.Build();


// 🔥 Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// 🔥 IMPORTANT: Area Routing (لازم يكون قبل العادي)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


// 🔥 Default Route (Customer)
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
);


app.Run();