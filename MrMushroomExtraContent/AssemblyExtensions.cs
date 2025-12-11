using System.Reflection;

namespace MrMushroomExtraContent;

public static class AssemblyExtensions
{
  public static string GetResourceByName(this Assembly assembly, string name)
  {
    foreach (var res in assembly.GetManifestResourceNames())
    {
      if (res.EndsWith("." + name))
        return res;
    }
    return null;
  }
}
