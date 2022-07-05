using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.WebApi.DAL;
using AMS.WebApi.Helpers;
using AMS.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMS.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AssetController : ControllerBase
  {
    private IAssetRepository _assetRepository;
    public AssetController(IAssetRepository assetRepository)
    {
      _assetRepository = assetRepository;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
      var assets = await _assetRepository.GetAssetsAsync();

      return Ok(assets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(int id)
    {
      var asset = await _assetRepository.GetAssetByIdAsync(id);

      if (asset == null)
      {
        return NotFound();
      }

      return Ok(asset);
    }

    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(Asset asset)
    {
      _assetRepository.AddAsset(asset);
      await _assetRepository.SaveAsync();

      return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Asset>> UpdateAsset(int id, Asset asset)
    {
      if (id != asset.Id)
      {
        return BadRequest();
      }

      var orgAsset = await _assetRepository.GetAssetByIdAsync(id);

      if (orgAsset == null)
      {
        return NotFound();
      }

      var mappedAsset = Utility.Map<Asset>(orgAsset, asset);
      _assetRepository.UpdateAsset(mappedAsset);

      try
      {
        await _assetRepository.SaveAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!IsAssetExist(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }


      return Ok(mappedAsset);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
      var asset = await _assetRepository.GetAssetByIdAsync(id);
      if (asset == null)
      {
        return NotFound();
      }

      _assetRepository.DeleteAsset(id);
      await _assetRepository.SaveAsync();

      return NoContent();
    }

    private bool IsAssetExist(int id)
    {
      return _assetRepository.IsExist(id);
    }
  }
}
