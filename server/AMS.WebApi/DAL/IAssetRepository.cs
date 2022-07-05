using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.WebApi.Models;

namespace AMS.WebApi.DAL
{
  public interface IAssetRepository : IDisposable
  {
    IEnumerable<Asset> GetAssets();
    Task<IEnumerable<Asset>> GetAssetsAsync();
    Asset GetAssetById(int id);
    Task<Asset> GetAssetByIdAsync(int id);
    void AddAsset(Asset asset);
    void DeleteAsset(int id);
    void DeleteAsset(Asset asset);
    void UpdateAsset(Asset asset);
    void Save();
    bool IsExist(int id);
    Task SaveAsync();
  }
}