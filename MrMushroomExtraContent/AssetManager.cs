using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace MrMushroomExtraContent;

public static class AssetManager
{
  private static AssetBundle assetBundle = null;

  public static IEnumerator LoadMrMushroomNpcAsync(Action<GameObject> onLoadCallback)
  {
    if (assetBundle == null)
    {
      var assembly = Assembly.GetExecutingAssembly();
      var resourceName = assembly.GetResourceByName("mrmushroomnpc");

      if (resourceName == null)
      {
        Plugin.Logger.LogError("Asset bundle 'mrmushroomnpc' not found");
        yield break;
      }

      Plugin.Logger.LogInfo("Loading asset bundle " + resourceName);
      using var stream = assembly.GetManifestResourceStream(resourceName);

      if (stream == null)
      {
        Plugin.Logger.LogError("Failed to load asset bundle " + resourceName);
        yield break;
      }

      var loadBundleRequest = AssetBundle.LoadFromStreamAsync(stream);
      yield return loadBundleRequest;

      assetBundle = loadBundleRequest.assetBundle;

      if (assetBundle == null)
      {
        Plugin.Logger.LogError($"Failed to load asset bundle '{resourceName}' from stream");
        yield break;
      }

      Plugin.Logger.LogInfo("Asset bundle loaded");
    }

    var loadAssetRequest = assetBundle.LoadAssetAsync<GameObject>("Mr Mushroom NPC");
    yield return loadAssetRequest;

    if (loadAssetRequest.asset == null)
    {
      Plugin.Logger.LogError("Failed to load 'Mr Mushroom NPC' from asset bundle");
      yield break;
    }

    onLoadCallback?.Invoke((GameObject)loadAssetRequest.asset);
  }

  public static void Cleanup()
  {
    if (assetBundle != null)
    {
      assetBundle.Unload(true);
      assetBundle = null;
      Plugin.Logger.LogInfo("Asset bundle unloaded");
    }
  }
}
