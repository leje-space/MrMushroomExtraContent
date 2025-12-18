using System.Collections;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace MrMushroomExtraContent;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
  public enum Phase
  {
    Phase1,
    Phase2,
    Phase3
  }

  private static readonly (Phase, string)[] PhaseScenes =
  {
    (Phase.Phase1, "Tut_03"),
    (Phase.Phase2, "Bone_05"),
    (Phase.Phase3, "Ant_02"),
  };

  internal static new ManualLogSource Logger;
  private Harmony harmony;

  private void Awake()
  {
    Logger = base.Logger;
    Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");

    harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    harmony.PatchAll(typeof(DialogueTextPatch));
    harmony.PatchAll(typeof(BossEncounterPatch));
    harmony.PatchAll(typeof(BattleStartPatch));
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
    foreach (var (phase, sceneName) in PhaseScenes)
    {
      if (scene.name == sceneName && ConditionsMet(phase))
      {
        StartCoroutine(Initialize(phase));
        break;
      }
    }
  }

  private static IEnumerator Initialize(Phase phase)
  {
    return phase switch
    {
      Phase.Phase1 => InitializeSceneManager.InitializePhase1Scene(),
      Phase.Phase2 => InitializeSceneManager.InitializePhase2Scene(),
      Phase.Phase3 => InitializeSceneManager.InitializePhase3Scene(),
      _ => null
    };
  }

  public static bool ConditionsMet(Phase phase)
  {
    var pd = PlayerData.instance;
    return phase switch
    {
      Phase.Phase1 => !pd.encounteredMossMother,
      Phase.Phase2 => !pd.encounteredBellBeast && pd.hasNeedleThrow,
      Phase.Phase3 => pd.defeatedBellBeast && !pd.blackThreadWorld && !Ant02GuardDefeated,
      _ => false
    };
  }

  private static bool Ant02GuardDefeated
  {
    get
    {
      var persistentBools = SceneData.instance.PersistentBools;
      return persistentBools.TryGetValue("Ant_02", "Battle Scene", out var value) && value.Value;
    }
  }
}
