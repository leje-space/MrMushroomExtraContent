using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace MrMushroomExtraContent;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
  internal static new ManualLogSource Logger;
  private Harmony harmony;

  private void Awake()
  {
    Logger = base.Logger;
    Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");

    harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    harmony.PatchAll(typeof(DialogueTextPatch));
    harmony.PatchAll(typeof(BossEncounterPatch));
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  private void OnDestroy()
  {
    harmony?.UnpatchSelf();
    AssetManager.Cleanup();
    Logger.LogInfo("Plugin resources cleaned up");
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (Phase1ConditionsMet && scene.name == "Tut_03")
    {
      InitializeSceneManager.InitializePhase1Scene();
    }
    else if (Phase2ConditionsMet && scene.name == "Bone_05")
    {
      StartCoroutine(InitializeSceneManager.InitializePhase2Scene());
    }
  }

  public static bool Phase1ConditionsMet
  {
    get
    {
      return !PlayerData.instance.encounteredMossMother;
    }
  }

  public static bool Phase2ConditionsMet
  {
    get
    {
      var playerData = PlayerData.instance;
      return !playerData.encounteredBellBeast && playerData.hasNeedleThrow;
    }
  }
}
