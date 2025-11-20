using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MrMushroomExtraContent;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
  internal static new ManualLogSource Logger;

  private void Awake()
  {
    Logger = base.Logger;
    Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");

    var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    harmony.PatchAll(typeof(InitializeScenePatch));
    harmony.PatchAll(typeof(DialogueTextPatch));
    harmony.PatchAll(typeof(StartBattlePatch));
  }

  public static bool Phase1ConditionsMet
  {
    get
    {
      return !PlayerData.instance.encounteredMossMother;
    }
  }
}
