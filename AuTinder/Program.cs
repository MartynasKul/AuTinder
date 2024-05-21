using AuTinder.Views;

namespace AuTinder
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddDistributedMemoryCache();
            // Add session middleware
            builder.Services.AddSession(options =>
            {
                // Set a timeout for the session
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
            });

            // Add PartsTechView service registration
            builder.Services.AddScoped<PartsTechView>();

            // Add HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Use session middleware

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseRouting();
            app.UseSession();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Route}/{action=Index}/{id?}");

            Config.CreateSingletonInstance(app.Configuration);


            app.Run();

        }
    }
}
