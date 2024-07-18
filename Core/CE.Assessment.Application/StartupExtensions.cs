using CE.Assessment.Application.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics.CodeAnalysis;

namespace CE.Assessment.Application;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<OrderService>();
        services.AddTransient<ProductsService>();

        return services;
    }

    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration config)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.ReadFrom.Configuration(config)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code).
            CreateBootstrapLogger();

        services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(config)
            .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

        services.AddHttpLogging(o => o.LoggingFields = HttpLoggingFields.All);

        return services;
    }
}