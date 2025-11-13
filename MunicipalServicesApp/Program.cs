using Microsoft.Extensions.Caching.Memory;
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

            // Services for RequestsService
            builder.Services.AddSingleton<RequestsService>();

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


/* POE PART 2
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

/* POE PART 3
 * Andrew Lock. (2024) An introduction to the heap data structure and .NET’s PriorityQueue. Available at: https://andrewlock.net/an-introduction-to-the-heap-data-structure-and-dotnets-priority-queue/ (Accessed: 4 November 2025).
 * Bootstrap. (2025) Cards · Bootstrap v5.0. Available at: https://getbootstrap.com/docs/5.0/components/card/ (Accessed: 6 November 2025).
 * C# Corner. (2025) Learn about Priority Queue in C# with examples. Available at: https://www.c-sharpcorner.com/article/learn-about-priority-queue-in-c-sharp-with-examples/ (Accessed: 8 November 2025).
 * Dev.to. (2021) Customizing .NET’s MVC template – Cards, images and transitions. Available at: https://dev.to/sbrevolution5/customizing-net-s-mvc-template-part-2-cards-images-and-transitions-4clo (Accessed: 5 November 2025).
 * DotNetTutorials. (2023) Sessions in ASP.NET Core MVC. Available at: https://dotnettutorials.net/lesson/sessions-in-asp-net-core-mvc/ (Accessed: 3 November 2025).
 * DotNetTutorials. (2024) How to implement file handling in ASP.NET Core MVC. Available at: https://dotnettutorials.net/lesson/file-handling-in-asp-net-core-mvc/ (Accessed: 4 November 2025).
 * GeeksforGeeks. (2025) Binary Search Tree (BST) – Data Structure. Available at: https://www.geeksforgeeks.org/dsa/binary-search-tree-data-structure/ (Accessed: 2 November 2025).
 * GeeksforGeeks. (2025) Introduction to Graph Data Structure and Algorithm Tutorials. Available at: https://www.geeksforgeeks.org/dsa/introduction-to-graphs-data-structure-and-algorithm-tutorials/ (Accessed: 6 November 2025).
 * Medium. (n.d.) Data Structures 101: Graphs — A Visual Introduction for Beginners. Available at: https://medium.com/free-code-camp/data-structures-101-graphs-a-visual-introduction-for-beginners-6d88f36ec768 (Accessed: 8 November 2025).
 * Medium. (n.d.) Data Structures: Heaps. Available at: https://medium.com/swlh/data-structures-heaps-b0398a521b (Accessed: 4 November 2025).
 * Microsoft. (2025) PriorityQueue<TElement, TPriority> Class. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-9.0 (Accessed: 5 November 2025).
 * Microsoft. (2025) Session and state management in ASP.NET Core. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0 (Accessed: 2 November 2025).
 * Microsoft. (2025) Upload files in ASP.NET Core. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-9.0 (Accessed: 4 November 2025).
 * StudyGlance. (n.d.) Introduction to Graph – Data Structures Tutorial. Available at: https://studyglance.in/ds/display.php?tno=32&topic=Introduction-to-Graph (Accessed: 7 November 2025).
 * W3Schools. (n.d.) DSA Graphs. Available at: https://www.w3schools.com/dsa/dsa_theory_graphs.php (Accessed: 7 November 2025).
 * Wikipedia. (2025) AVL tree. Available at: https://en.wikipedia.org/wiki/AVL_tree (Accessed: 3 November 2025).
 */