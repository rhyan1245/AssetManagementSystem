using System;
using System.Reflection;

namespace AMS.WebApi.Helpers
{
  public static class Utility
  {
    public static T Map<T>(T destination, object source)
    {
      if (!source.GetType().IsAssignableFrom(typeof(T)))
      {
        throw new Exception($"Mapping source and desination type must be same: source type [{source.GetType().FullName}] is not same as destination type [{typeof(T).FullName}]");
      }

      var properties = source.GetType().GetProperties();
      foreach (PropertyInfo p in properties)
      {
        var value = p.GetValue(source);
        var destProperty = destination.GetType().GetProperty(p.Name);
        if (destProperty != null && value != null)
        {
          destProperty.SetValue(destination, value);
        }
      }

      return destination;
    }
  }
}