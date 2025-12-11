using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TeamCherry.Localization;

namespace MrMushroomExtraContent;

public static class LocalizationManager
{
  private static readonly Dictionary<LanguageCode, Dictionary<string, string>> translations = new();
  private static bool initialized = false;

  public static void Initialize()
  {
    if (initialized) return;

    LoadLanguage(LanguageCode.EN, "en");
    LoadLanguage(LanguageCode.RU, "ru");

    initialized = true;
  }

  public static string GetText(string key)
  {
    return GetText(key, Language.CurrentLanguage());
  }

  public static string GetText(string key, LanguageCode language)
  {
    if (!initialized)
    {
      Initialize();
    }

    if (translations.TryGetValue(language, out var langDict) &&
        langDict.TryGetValue(key, out var text))
    {
      return text;
    }

    // Fallback to English
    if (language != LanguageCode.EN &&
        translations.TryGetValue(LanguageCode.EN, out var enDict) &&
        enDict.TryGetValue(key, out var enText))
    {
      return enText;
    }

    Plugin.Logger.LogWarning($"Missing localization for key '{key}'");
    return key;
  }

  private static void LoadLanguage(LanguageCode code, string fileName)
  {
    var assembly = Assembly.GetExecutingAssembly();
    var resourceName = assembly.GetResourceByName($"localization.{fileName}.json");

    if (resourceName == null)
    {
      Plugin.Logger.LogWarning($"Localization file '{fileName}.json' not found");
      return;
    }

    using var stream = assembly.GetManifestResourceStream(resourceName);
    if (stream == null)
    {
      Plugin.Logger.LogError($"Failed to load localization stream for '{fileName}'");
      return;
    }

    using var reader = new StreamReader(stream);
    var json = reader.ReadToEnd();

    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    if (dict != null)
    {
      translations[code] = dict;
      Plugin.Logger.LogDebug($"Loaded {dict.Count} strings for language '{code}'");
    }
  }
}
