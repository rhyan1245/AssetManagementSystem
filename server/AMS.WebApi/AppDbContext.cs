using AMS.WebApi.Models;
using AMS.WebApi.Models.EnumTypes;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AMS.WebApi
{
  public class AppDbContext : IdentityDbContext<IdentityUser>
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
        entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
      });
    }
  }
}