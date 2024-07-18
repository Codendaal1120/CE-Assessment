using CE.Assessment.Application;
using CE.Assessment.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace CE.Assessment.Web;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        builder.Services.AddLogging(config);
        builder.Services.AddControllersWithViews().AddNToastNotifyToastr();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(config);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseNToastNotify();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
