using HarmonyLib;
using HutongGames.PlayMaker.Actions;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(DisplayBossTitle), nameof(DisplayBossTitle.OnEnter))]
class BossEncounterPatch
{
  static void Postfix()
  {
    try
    {
      BattleStartHandler.Handle();
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in BossEncounterPatch: {ex}");
    }
  }
}
