using HarmonyLib;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(BattleScene), nameof(BattleScene.StartBattle))]
class StartBattlePatch
{
  static void Postfix()
  {
    try
    {
      if (!Plugin.Phase1ConditionsMet)
        return;

      var npc = GameObjectFinder.FindMrMushroomNpc();
      if (npc == null)
      {
        Plugin.Logger.LogDebug("Mr Mushroom not found during battle start");
        return;
      }

      var fsm = npc.GetComponent<PlayMakerFSM>();
      if (fsm == null)
      {
        Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom during battle start");
        return;
      }

      if (fsm.ActiveStateName == "Appearing" || fsm.ActiveStateName == "Idle")
      {
        // Proceed to state Disappearing
        fsm.SendEvent("CANCEL");
      }

      var playerDetector = GameObjectFinder.FindPlayerDetector();
      if (playerDetector == null)
      {
        Plugin.Logger.LogWarning("PlayerDetector component not found during battle start");
      }
      else
      {
        // Mr Mushroom's appearance no longer needed
        playerDetector.enabled = false;
      }
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in StartBattlePatch: {ex}");
    }
  }
}
