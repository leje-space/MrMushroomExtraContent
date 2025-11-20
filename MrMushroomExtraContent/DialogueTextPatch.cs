using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using TeamCherry.Localization;

namespace MrMushroomExtraContent;

[HarmonyPatch(typeof(RunDialogue), nameof(RunDialogue.DialogueText), MethodType.Getter)]
class DialogueTextPatch
{
  private const string MRMUSH_LOC1_TEXT_EN = "...Tell me, wanderer... is there a place in this world for an object of such extraordinary power?<page>" +
    "...When moving the underground paths, does anyone remember that those paths once led in a completely different direction?<hpage>" +
    "You’re called the Herald, aren’t you?<page>" +
    "...But if there is only one road, who can say what it will become?<page>";
  private const string MRMUSH_LOC1_TEXT_RU = "...Скажи мне, странник... есть ли место в мире предмету столь необычайной силы?<page>" +
    "...Двигаясь подземными тропами, помнит ли кто о том, что вели те тропы в совершенно ином направлении?<hpage>" +
    "Вас зовут Глашатай, не так ли?<page>" +
    "...Но если дорога одна, кто знает, чем она обернётся?<page>";

  static bool Prefix(RunDialogue __instance, ref string __result)
  {
    try
    {
      if (__instance.Sheet.Value == "Wanderers" &&
          __instance.Key.Value == "MRMUSH_LOC1" &&
          Plugin.Phase1ConditionsMet)
      {
        __result = GetMrMushLoc1Text();
        return false; // skip original method
      }
      return true;
    }
    catch (System.Exception ex)
    {
      Plugin.Logger.LogError($"Error in DialogueTextPatch: {ex}");
      return true; // run original method on error
    }
  }

  private static string GetMrMushLoc1Text()
  {
    switch (Language.CurrentLanguage())
    {
      case LanguageCode.RU:
        return MRMUSH_LOC1_TEXT_RU;
      default:
        return MRMUSH_LOC1_TEXT_EN;
    }
  }
}
