using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AMS.WebApi.Models;
using AMS.WebApi.Models.EnumTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AMS.WebApi.Seeders
{
  public class AssetDataGenerator
  {
    private AppDbContext _context;

    public AssetDataGenerator(AppDbContext context)
    {
      _context = context;
    }

    public List<Asset> GetAssets()
    {
      return new List<Asset>
      {
        new Asset
        {
          Name = "Laptop",
          Type = AssetType.Hardware,
          Description = "Ullamco excepteur consequat mollit amet tempor."
        },
        new Asset
        {
          Name = "Mouse",
          Type = AssetType.Hardware,
          Description = "Ut magna ex aliqua pariatur cillum."
        },
        new Asset
        {
          Name = "Windows 10",
          Type = AssetType.Software,
          Description = "Eiusmod aute exercitation deserunt sit elit pariatur est eiusmod non ut incididunt amet."
        }
      };
    }

    public static void Generate(IServiceProvider serviceProvider)
    {
      using (var context = new AppDbContext(
                  serviceProvider.GetRequiredService<
                      DbContextOptions<AppDbContext>>()))
      {
        context.Database.EnsureCreated();

        if (context.Assets.Any())
        {
          return;
        }

        AssetDataGenerator generator = new AssetDataGenerator(context);

        var assets = generator.GetAssets();
        foreach (var asset in assets)
        {
          context.Assets.Add(asset);
        }
        context.SaveChanges();
      }
    }

    public static void GenerateUsers(IServiceProvider serviceProvider)
    {
      using (var context = new AppDbContext(
                  serviceProvider.GetRequiredService<
                      DbContextOptions<AppDbContext>>()))
      {
        context.Database.EnsureCreated();

        if (context.Users.Any())
        {
          return;
        }

        var userMgr = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var johndoe = userMgr.FindByNameAsync("johndoe").Result;
        if (johndoe == null)
        {
          johndoe = new IdentityUser
          {
            UserName = "johndoe@example.com",
            Email = "johndoe@example.com",
            EmailConfirmed = true
          };

          var result = userMgr.CreateAsync(johndoe, "JohnDoe@123").Result;
          if (!result.Succeeded)
          {
            throw new Exception(result.Errors.First().Description);
          }

          result = userMgr.AddClaimsAsync(johndoe, new List<Claim> {
           new Claim("Name", "John Doe"),
           new Claim("GivenName", "John")
        }).Result;

          if (!result.Succeeded)
          {
            throw new Exception(result.Errors.First().Description);
          }
        }

      }
    }
  }
}