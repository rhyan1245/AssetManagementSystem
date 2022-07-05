using Microsoft.EntityFrameworkCore;
using AMS.WebApi.Models;
using AMS.WebApi.Models.EnumTypes;
using System;
using Microsoft.Extensions.Logging;

namespace AMS.WebApi
{
  public class AppDbContext : DbContext
  {
    public DbSet<Asset> Assets => Set<Asset>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Asset>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Type)
        .HasConversion(v => v.ToString(), v => (AssetType)Enum.Parse(typeof(AssetType), v));
      });
    }
  }
}