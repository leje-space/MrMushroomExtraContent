using HarmonyLib;
using HutongGames.PlayMaker.Actions;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(RunDialogue), nameof(RunDialogue.DialogueText), MethodType.Getter)]
class DialogueTextPatch
{
  static bool Prefix(RunDialogue __instance, ref string __result)
  {
    try
    {
      if (__instance.Sheet.Value != "Wanderers" ||
          __instance.Key.Value != "MRMUSH_LOC1")
      {
        return true;
      }

      if (Plugin.Phase1ConditionsMet)
      {
        __result = LocalizationManager.GetText("MRMUSH_PHASE1");
      }
      else if (Plugin.Phase2ConditionsMet)
      {
        __result = LocalizationManager.GetText("MRMUSH_PHASE2");
      }
      else
      {
        return true;
      }
      return false; // skip original method
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in DialogueTextPatch: {ex}");
      return true; // run original method on error
    }
  }
}
