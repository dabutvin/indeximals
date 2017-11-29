using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Indeximals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            });

            return builder
                .UseStartup<Startup>()
                .Build();
        }
    }
}
