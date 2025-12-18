using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(RunDialogue), nameof(RunDialogue.DialogueText), MethodType.Getter)]
class DialogueTextPatch
{
  static readonly string[] Keys = [
    "MRMUSH_PHASE1",
    "MRMUSH_PHASE2",
    "MRMUSH_PHASE3",
  ];

  static bool Prefix(RunDialogue __instance, ref string __result)
  {
    try
    {
      if (__instance.Sheet.Value != "Wanderers" ||
          !Keys.Contains(__instance.Key.Value))
      {
        return true;
      }

      __result = LocalizationManager.GetText(__instance.Key.Value);
      return false; // skip original method
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in DialogueTextPatch: {ex}");
      return true; // run original method on error
    }
  }
}
