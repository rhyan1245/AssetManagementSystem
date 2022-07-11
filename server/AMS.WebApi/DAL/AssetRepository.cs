using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.WebApi.Extensions;
using AMS.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.WebApi.DAL
{
  public class AssetRepository : IAssetRepository
  {
    private bool disposedValue = false;
    private AppDbContext _context;

    public AssetRepository(AppDbContext context)
    {
      _context = context;
    }

    public void AddAsset(Asset asset)
    {
      _context.Assets.Add(asset);
    }

    public void DeleteAsset(Asset asset)
    {
      _context.Assets.Remove(asset);
    }

    public Asset GetAssetById(int id, string userId)
    {
      return _context.Assets.Include(c => c.User).ForUser(userId).FirstOrDefault(c => c.Id == id);
    }

    public async Task<Asset> GetAssetByIdAsync(int id, string userId)
    {
      return await _context.Assets.Include(c => c.User).ForUser(userId).FirstOrDefaultAsync(c => c.Id == id);
    }

    public IEnumerable<Asset> GetAssets(string userId)
    {
      return _context.Assets.Include(c => c.User).ForUser(userId).ToList();
    }

    public async Task<IEnumerable<Asset>> GetAssetsAsync(string userId)
    {
      return await _context.Assets.Include(c => c.User).ForUser(userId).ToListAsync();
    }

    public void Save()
    {
      _context.SaveChanges();
    }

    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }

    public void UpdateAsset(Asset asset)
    {
      _context.Entry(asset).State = EntityState.Modified;
    }

    public bool IsExist(int id)
    {
      return _context.Assets.Any(e => e.Id == id);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          _context.Dispose();
        }
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      System.GC.SuppressFinalize(this);
    }
  }
}