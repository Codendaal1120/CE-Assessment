using CE.Assessment.Infrastructure.WebClients.ChannelEngine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Diagnostics.CodeAnalysis;

namespace CE.Assessment.Infrastructure;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddRefitClient<IChannelEngineClient>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(config.GetValue<string>("ChannelEngine:ApiUrl") ?? string.Empty);
            c.DefaultRequestHeaders.Add("X-CE-KEY", config.GetValue<string>("ChannelEngine:ApiKey") ?? string.Empty);
        });

        return services;
    }
}
