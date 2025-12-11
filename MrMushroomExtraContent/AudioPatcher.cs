using UnityEngine;
using UnityEngine.Audio;

namespace MrMushroomExtraContent;

public static class AudioPatcher
{
  public static bool PatchMrMushroomNpc(GameObject npc)
  {
    var audioSource = npc.GetComponent<AudioSource>();
    if (audioSource == null)
    {
      Plugin.Logger.LogError("AudioSource component not found on Mr Mushroom");
      return false;
    }

    var audioMixerGroup = FindActorsAudioMixerGroup();
    if (audioMixerGroup == null)
    {
      Plugin.Logger.LogError("AudioMixerGroup 'Actors' not found");
      return false;
    }

    audioSource.outputAudioMixerGroup = audioMixerGroup;
    Plugin.Logger.LogDebug("Fixed AudioSource for Mr Mushroom");

    return true;
  }

  private static AudioMixerGroup FindActorsAudioMixerGroup()
  {
    var objects = Resources.FindObjectsOfTypeAll<AudioMixerGroup>();

    foreach (var obj in objects)
    {
      if (obj.name == "Actors")
      {
        return obj;
      }
    }

    return null;
  }
}
