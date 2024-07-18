using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CE.Assessment.Application;
using CE.Assessment.Infrastructure;
using System.Reflection;

namespace CE.Assessment;

internal class Program
{
    static async Task Main()
    {
        var serviceProvider = Setup();

        while (true)
        {
            Console.WriteLine("Enter command or type help for help information");
            await serviceProvider.GetRequiredService<ComandLineRunner>().Run(Console.ReadLine());
        }
    }

    private static ServiceProvider Setup()
    {
        var config = CreateConfig();

        var sp = new ServiceCollection()
            .AddLogging(config)
            .AddApplicationServices()
            .AddInfrastructureServices(config)
            .AddTransient<ComandLineRunner>()
            .BuildServiceProvider();

        return sp;
    }

    private static IConfiguration CreateConfig()
    {
        var config = new Dictionary<string, string>
        {
            {"ChannelEngine:ApiUrl", "https://api-dev.channelengine.net/api"},
            {"ChannelEngine:ApiKey", "StoredInSecret"},
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        return configuration;
    }
}
