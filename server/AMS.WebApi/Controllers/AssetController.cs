using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AMS.WebApi.DAL;
using AMS.WebApi.Helpers;
using AMS.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AMS.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class AssetController : ControllerBase
  {
    private IAssetRepository _assetRepository;
    private readonly UserManager<IdentityUser> _userManager;
    public AssetController(IAssetRepository assetRepository, UserManager<IdentityUser> userManager)
    {
      _assetRepository = assetRepository;
      _userManager = userManager;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var assets = await _assetRepository.GetAssetsAsync(userId);

      return Ok(assets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var asset = await _assetRepository.GetAssetByIdAsync(id, userId);

      if (asset == null)
      {
        return NotFound();
      }

      return Ok(asset);
    }

    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(Asset asset)
    {
      var user = await _userManager.GetUserAsync(User);
      asset.User = user;
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

      string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var orgAsset = await _assetRepository.GetAssetByIdAsync(id, userId);

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
      string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var asset = await _assetRepository.GetAssetByIdAsync(id, userId);
      if (asset == null)
      {
        return NotFound();
      }

      _assetRepository.DeleteAsset(asset);
      await _assetRepository.SaveAsync();

      return NoContent();
    }

    private bool IsAssetExist(int id)
    {
      return _assetRepository.IsExist(id);
    }
  }
}
