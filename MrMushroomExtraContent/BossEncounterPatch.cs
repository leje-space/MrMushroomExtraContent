using System;
using System.Collections;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(DisplayBossTitle), nameof(DisplayBossTitle.OnEnter))]
class BossEncounterPatch
{
  static void Postfix()
  {
    try
    {
      if (!Plugin.Phase1ConditionsMet && !Plugin.Phase2ConditionsMet)
        return;

      var npc = GameObjectFinder.FindMrMushroomNpc();
      if (npc == null)
      {
        Plugin.Logger.LogDebug("Mr Mushroom not found during boss encounter");
        return;
      }

      var fsm = npc.GetComponent<PlayMakerFSM>();
      if (fsm == null)
      {
        Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom during boss encounter");
        return;
      }

      Plugin.Logger.LogDebug("Mr Mushroom's state: " + fsm.ActiveStateName);
      if (fsm.ActiveStateName == "Appearing" || fsm.ActiveStateName == "Idle")
      {
        // Proceed to state Disappearing
        fsm.SendEvent("CANCEL");
      }
      else if (fsm.ActiveStateName == "To Idle")
      {
        // Proceed to state Disappearing after waiting
        fsm.StartCoroutine(PostponeAction(0.5f, () =>
        {
          // Check if fsm still exists and is valid
          if (fsm != null && fsm.gameObject != null && fsm.ActiveStateName == "Idle")
          {
            fsm.SendEvent("CANCEL");
          }
        }));
      }

      var playerDetector = GameObjectFinder.FindPlayerDetector();
      if (playerDetector != null)
      {
        // Mr Mushroom's appearance no longer needed
        playerDetector.enabled = false;
      }
      else
      {
        Plugin.Logger.LogWarning("PlayerDetector component not found during boss encounter");
      }
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in BossEncounterPatch: {ex}");
    }
  }

  private static IEnumerator PostponeAction(float waitTime, Action callback)
  {
    yield return new WaitForSeconds(waitTime);

    callback?.Invoke();
  }
}
