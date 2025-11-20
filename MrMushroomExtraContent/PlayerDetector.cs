using System.Collections;
using UnityEngine;

namespace MrMushroomExtraContent;

public class PlayerDetector : MonoBehaviour
{
  private bool roared = false;

  private void OnTriggerEnter2D(Collider2D other)
  {
    try
    {
      if (!enabled || !Plugin.Phase1ConditionsMet)
        return;

      if (!other.CompareTag("Player"))
        return;

      var npcTransform = gameObject.transform.parent;
      if (npcTransform == null)
      {
        Plugin.Logger.LogError("Mr Mushroom not found in PlayerDetector");
        return;
      }

      var npc = npcTransform.gameObject;
      var fsm = npc.GetComponent<PlayMakerFSM>();
      if (fsm == null)
      {
        Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom in PlayerDetector");
        return;
      }

      // Proceed to state Appearing
      fsm.SendEvent("SING");

      if (!roared)
      {
        var hero = other.gameObject;
        var heroController = hero.GetComponent<HeroController>();
        if (heroController == null)
        {
          Plugin.Logger.LogError("HeroController component not found on player in PlayerDetector");
          return;
        }

        heroController.StartRoarLockNoRecoil();
        StartCoroutine(StopRoarLockCoroutine(heroController));
        roared = true;
      }
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in PlayerDetector.OnTriggerEnter2D: {ex}");
    }
  }

  private IEnumerator StopRoarLockCoroutine(HeroController heroController)
  {
    yield return new WaitForSeconds(1f);

    // Check if heroController still exists and is valid
    if (heroController != null && heroController.gameObject != null)
    {
      heroController.StopRoarLock();
    }
  }
}
