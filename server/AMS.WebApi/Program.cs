using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.WebApi.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AMS.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var app = CreateHostBuilder(args).Build();

      using (var scope = app.Services.CreateScope())
      {
        var services = scope.ServiceProvider;

        AssetDataGenerator.GenerateUsers(services);
        AssetDataGenerator.Generate(services);
      }

      app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
