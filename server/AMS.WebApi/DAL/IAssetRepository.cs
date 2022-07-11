using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.WebApi.Models;

namespace AMS.WebApi.DAL
{
  public interface IAssetRepository : IDisposable
  {
    IEnumerable<Asset> GetAssets(string userId);
    Task<IEnumerable<Asset>> GetAssetsAsync(string userId);
    Asset GetAssetById(int id, string userId);
    Task<Asset> GetAssetByIdAsync(int id, string userId);
    void AddAsset(Asset asset);
    void DeleteAsset(Asset asset);
    void UpdateAsset(Asset asset);
    void Save();
    bool IsExist(int id);
    Task SaveAsync();
  }
}