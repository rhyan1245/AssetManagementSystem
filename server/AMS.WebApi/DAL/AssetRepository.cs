using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public void DeleteAsset(int id)
    {
      Asset asset = GetAssetById(id);
      DeleteAsset(asset);
    }

    public void DeleteAsset(Asset asset)
    {
      _context.Assets.Remove(asset);
    }

    public Asset GetAssetById(int id)
    {
      return _context.Assets.Find(id);
    }

    public async Task<Asset> GetAssetByIdAsync(int id)
    {
      return await _context.Assets.FindAsync(id);
    }

    public IEnumerable<Asset> GetAssets()
    {
      return _context.Assets.ToList();
    }

    public async Task<IEnumerable<Asset>> GetAssetsAsync()
    {
      return await _context.Assets.ToListAsync();
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