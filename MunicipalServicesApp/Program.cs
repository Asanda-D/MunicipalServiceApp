using MunicipalServicesApp.Services;

namespace MunicipalServicesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register custom services
            builder.Services.AddSingleton<EventsService>();

            // Register HttpContextAccessor (required for accessing HttpContext in views/controllers)
            builder.Services.AddHttpContextAccessor();

            // Configure session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add session BEFORE authorization
            app.UseSession();

            app.UseAuthorization();

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

/*
 * Microsoft. (2025) Get started with EF Core in an ASP.NET MVC web app. Available at: https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-9.0 (Accessed: 4 September 2025).
 * Microsoft. (2025) Part 4, add a model to an ASP.NET Core MVC app. Available at: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-9.0 (Accessed: 5 September 2025).
 * Microsoft. (2024) Create C# ASP.NET Core web application - Visual Studio. Available at: https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-aspnet-core?view=vs-2022 (Accessed: 6 September 2025).
 * Microsoft. (2024) Create web APIs with ASP.NET Core. Available at: https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-9.0 (Accessed: 7 September 2025).
 * Microsoft. (2025) ASP.NET Core fundamentals overview. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-9.0 (Accessed: 8 September 2025).
 * Microsoft. (2025) Quickstart: Deploy an ASP.NET web app - Azure App Service. Available at: https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore (Accessed: 9 September 2025).
 * Stack Overflow. (2014) How to display errors with ASP.NET Core. Available at: https://stackoverflow.com/questions/24563493/how-to-display-errors-with-asp-net-core (Accessed: 4 September 2025).
 * Stack Overflow. (2011) How to create a function in a cshtml template?. Available at: https://stackoverflow.com/questions/6531983/how-to-create-a-function-in-a-cshtml-template (Accessed: 5 September 2025).
 * Stack Overflow. (2020) ASP.NET Core, unable to find View of cshtml when published. Available at: https://stackoverflow.com/questions/64175077/asp-net-core-unable-to-find-view-of-cshtml-when-published (Accessed: 6 September 2025).
 * TutorialsTeacher. (2025) Program.cs in ASP.NET Core MVC. Available at: https://www.tutorialsteacher.com/core/aspnet-core-program (Accessed: 7 September 2025).
 */