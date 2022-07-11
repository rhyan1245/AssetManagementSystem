using Microsoft.AspNetCore.Identity;

namespace AMS.WebApi.Models
{
  public class Asset
  {
    public int? Id { get; set; }
    public string Name { get; set; }
    public EnumTypes.AssetType? Type { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
  }
}