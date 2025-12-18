using System.Collections;
using Core.FsmUtil;
using HutongGames.PlayMaker.Actions;
using QuestPlaymakerActions;
using UnityEngine;

namespace MrMushroomExtraContent;

public static class InitializeSceneManager
{
  public static IEnumerator InitializePhase1Scene()
  {
    Plugin.Logger.LogInfo("Initializing Phase 1 scene");

    var npc = GameObjectFinder.FindMrMushroomNpc();
    if (npc == null)
    {
      Plugin.Logger.LogWarning("Mr Mushroom not found");
      yield break;
    }

    if (!InitializeMrMushroomNpc(npc, dialogueTextKey: "MRMUSH_PHASE1"))
      yield break;

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

    if (!InitializeMrMushroomNpc(npc, dialogueTextKey: "MRMUSH_PHASE2"))
      yield break;

    Plugin.Logger.LogInfo("Scene initialized successfully");
  }

  public static IEnumerator InitializePhase3Scene()
  {
    Plugin.Logger.LogInfo("Initializing Phase 3 scene");

    var guard = GameObject.Find("Bone Hunter Throw");
    if (guard == null)
    {
      Plugin.Logger.LogError("Guard not found");
      yield break;
    }

    if (!InitializeAnt02Guard(guard))
      yield break;

    GameObject npcPrefab = null;
    yield return AssetManager.LoadMrMushroomNpcAsync((loadedPrefab) => npcPrefab = loadedPrefab);

    if (npcPrefab == null)
      yield break;

    var npc = Object.Instantiate(npcPrefab);
    npc.transform.position = new Vector3(48.36f, 5.352f, 0.006f);

    Plugin.Logger.LogDebug($"Instantiated Mr Mushroom at position: {npc.transform.position}");

    if (!ShaderPatcher.PatchMrMushroomNpc(npc))
      yield break;

    if (!AudioPatcher.PatchMrMushroomNpc(npc))
      yield break;

    if (!InitializeMrMushroomNpc(npc, hidden: false, dialogueTextKey: "MRMUSH_PHASE3"))
      yield break;

    Plugin.Logger.LogInfo("Scene initialized successfully");
  }

  private static bool InitializeMrMushroomNpc(GameObject npc, bool hidden = true, string dialogueTextKey = null)
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
    fsm.RemoveAction("Talk B", 0);

    fsm.RemoveTransition("Hidden", "SING");
    fsm.RemoveTransition("Disappearing", "SING");

    fsm.AddTransition("Hidden", "APPEAR", "Appearing");
    fsm.AddTransition("Disappearing", "APPEAR", "Appearing");
    fsm.AddTransition("Appearing", "DISAPPEAR", "Disappearing");
    fsm.AddTransition("Idle", "DISAPPEAR", "Disappearing");

    if (!hidden)
    {
      fsm.ChangeTransition("Check", "FINISHED", "Idle");
      fsm.RemoveTransition("Idle", "CANCEL");

      // Activate Mr Mushroom's Hit Detect
      var activateGameObject = fsm.GetAction<ActivateGameObject>("Init", 7);
      activateGameObject.activate = true;

      var needolinAppearTimeline = npc.GetComponentInChildren<NeedolinAppearTimeline>(true);
      needolinAppearTimeline.currentTime = needolinAppearTimeline.maxTime;
    }

    if (dialogueTextKey != null)
    {
      fsm.FsmVariables.FindFsmString("Meet Cell").Value = dialogueTextKey;

      if (dialogueTextKey == "MRMUSH_PHASE3")
      {
        fsm.ChangeTransition("Talk Type", "FINISHED", "Talk B");
      }
    }

    npc.SetActive(true);

    if (hidden)
    {
      var triggerAreaTransform = npc.transform.Find("Close Range");
      if (triggerAreaTransform == null)
      {
        Plugin.Logger.LogError("Trigger area 'Close Range' not found on Mr Mushroom");
        return false;
      }

      var triggerArea = triggerAreaTransform.gameObject;
      triggerArea.AddComponent<PlayerDetector>();
    }

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

  private static bool InitializeAnt02Guard(GameObject guard)
  {
    var wakeRangeTransform = guard.transform.Find("Wake Range");
    if (wakeRangeTransform == null)
    {
      Plugin.Logger.LogError("Wake Range not found on Guard");
      return false;
    }

    wakeRangeTransform.gameObject.SetActive(false);

    var guardRange = GameObject.Find("Guard Range");
    if (guardRange == null)
    {
      Plugin.Logger.LogError("Guard Range not found");
      return false;
    }

    guardRange.SetActive(false);

    var eventRegister = guard.AddComponent<EventRegister>();
    eventRegister.SubscribedEvent = "MR MUSHROOM LEFT";

    var fsm = guard.GetComponent<PlayMakerFSM>();
    if (fsm == null)
    {
      Plugin.Logger.LogError("PlayMakerFSM component not found on Guard");
      return false;
    }

    fsm.AddTransition("Rest", "MR MUSHROOM LEFT", "Wake");

    return true;
  }
}
