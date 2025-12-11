using System.Collections;
using Core.FsmUtil;
using HutongGames.PlayMaker.Actions;
using QuestPlaymakerActions;
using UnityEngine;

namespace MrMushroomExtraContent;

public static class InitializeSceneManager
{
  public static void InitializePhase1Scene()
  {
    Plugin.Logger.LogInfo("Initializing Phase 1 scene");

    var npc = GameObjectFinder.FindMrMushroomNpc();
    if (npc == null)
    {
      Plugin.Logger.LogWarning("Mr Mushroom not found");
      return;
    }

    if (!InitializeMrMushroomNpc(npc))
      return;

    Plugin.Logger.LogInfo("Scene initialized successfully");
  }

  public static IEnumerator InitializePhase2Scene()
  {
    Plugin.Logger.LogInfo("Initializing Phase 2 scene");

    GameObject npcPrefab = null;
    yield return AssetManager.LoadMrMushroomNpcAsync((loadedPrefab) => npcPrefab = loadedPrefab);

    if (npcPrefab == null)
      yield break;

    var npc = Object.Instantiate(npcPrefab);
    npc.transform.position = new Vector3(77.44f, 4.352f, 0.006f);

    Plugin.Logger.LogDebug($"Instantiated Mr Mushroom at position: {npc.transform.position}");

    if (!ShaderPatcher.PatchMrMushroomNpc(npc))
      yield break;

    if (!AudioPatcher.PatchMrMushroomNpc(npc))
      yield break;

    if (!InitializeMrMushroomNpc(npc))
      yield break;

    Plugin.Logger.LogInfo("Scene initialized successfully");
  }

  private static bool InitializeMrMushroomNpc(GameObject npc)
  {
    var fsm = npc.GetComponent<PlayMakerFSM>();
    if (fsm == null)
    {
      Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom");
      return false;
    }

    // Proceed even if quest is not accepted
    var checkQuestState = fsm.GetAction<CheckQuestStateV2>("Check", 0);
    checkQuestState.NotTrackedEvent = null;

    // Do not cancel state if hero is not performing
    var checkHeroPerformanceRegion = fsm.GetAction<CheckHeroPerformanceRegion>("Appearing", 0);
    checkHeroPerformanceRegion.None = null;

    // Do not set MushroomQuestFound1 = true
    fsm.RemoveAction("Talk A1", 0);

    npc.SetActive(true);

    var triggerAreaTransform = npc.transform.Find("Close Range");
    if (triggerAreaTransform == null)
    {
      Plugin.Logger.LogError("Trigger area 'Close Range' not found on Mr Mushroom");
      return false;
    }

    var triggerArea = triggerAreaTransform.gameObject;
    triggerArea.AddComponent<PlayerDetector>();

    var ambientShroomSet = GameObject.Find("Mr_Mushroom_Ambient_Shroom_Set");
    if (ambientShroomSet != null)
    {
      ambientShroomSet.SetActive(false);
    }
    else
    {
      Plugin.Logger.LogDebug("Ambient shroom set not found or inactive");
    }

    return true;
  }
}
