using Core.FsmUtil;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using QuestPlaymakerActions;
using UnityEngine;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(HeroController), nameof(HeroController.FinishedEnteringScene))]
class InitializeScenePatch
{
  static void Postfix(HeroController __instance)
  {
    try
    {
      if (!Plugin.Phase1ConditionsMet)
        return;

      if (GameManager.instance.sceneName != "Tut_03")
        return;

      Plugin.Logger.LogInfo("Initializing scene");

      var npc = GameObjectFinder.FindMrMushroomNpc();
      if (npc == null)
      {
        Plugin.Logger.LogWarning("Mr Mushroom not found");
        return;
      }

      var fsm = npc.GetComponent<PlayMakerFSM>();
      if (fsm == null)
      {
        Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom");
        return;
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
        return;
      }

      var triggerArea = triggerAreaTransform.gameObject;
      triggerArea.AddComponent<PlayerDetector>();

      Plugin.Logger.LogInfo("Scene initialized successfully");
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in InitializeScenePatch: {ex}");
    }
  }
}
