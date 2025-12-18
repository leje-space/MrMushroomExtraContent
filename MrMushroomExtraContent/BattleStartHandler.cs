using System;
using System.Collections;
using UnityEngine;

namespace MrMushroomExtraContent;

public static class BattleStartHandler
{
  public static void Handle()
  {
    var npc = GameObjectFinder.FindMrMushroomNpc();
    if (npc == null)
      return;

    Plugin.Logger.LogDebug("Mr Mushroom found during battle start");

    var fsm = npc.GetComponent<PlayMakerFSM>();
    if (fsm == null)
    {
      Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom during battle start");
      return;
    }

    Plugin.Logger.LogDebug("Mr Mushroom's state: " + fsm.ActiveStateName);
    if (fsm.ActiveStateName == "Appearing" || fsm.ActiveStateName == "Idle")
    {
      fsm.SendEvent("DISAPPEAR");
    }
    else if (fsm.ActiveStateName == "To Idle")
    {
      fsm.StartCoroutine(PostponeAction(0.5f, () =>
      {
        // Check if fsm still exists and is valid
        if (fsm != null && fsm.gameObject != null && fsm.ActiveStateName == "Idle")
        {
          fsm.SendEvent("DISAPPEAR");
        }
      }));
    }

    var playerDetector = GameObjectFinder.FindPlayerDetector();
    if (playerDetector != null)
    {
      Plugin.Logger.LogDebug("PlayerDetector component found during battle start");

      // Mr Mushroom's appearance no longer needed
      playerDetector.enabled = false;
    }
  }

  private static IEnumerator PostponeAction(float waitTime, Action callback)
  {
    yield return new WaitForSeconds(waitTime);

    callback?.Invoke();
  }
}
