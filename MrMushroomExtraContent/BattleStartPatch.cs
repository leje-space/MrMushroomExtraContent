using HarmonyLib;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(BattleScene), nameof(BattleScene.StartBattle))]
class BattleStartPatch
{
  static void Postfix()
  {
    try
    {
      BattleStartHandler.Handle();
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in BattleStartPatch: {ex}");
    }
  }
}
