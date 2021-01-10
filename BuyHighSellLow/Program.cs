using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BuyHighSellLow
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureWebHost(c => c.UseUrls("http://localhost:5000"))
                .UseWindowsService();
    }
}