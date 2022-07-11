using System.Linq;
using AMS.WebApi.Models;

namespace AMS.WebApi.Extensions
{
  public static class QueryExtension
  {
    public static IQueryable<Asset> ForUser(this IQueryable<Asset> query, string userId)
    {
      return query.Where(x => x.UserId == userId);
    }
  }
}