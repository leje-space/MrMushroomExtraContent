using Core.FsmUtil;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace MrMushroomExtraContent;

public static class ShaderPatcher
{
  public static bool PatchMrMushroomNpc(GameObject npc)
  {
    var (defaultShader, litShader, colorFlashShader) = FindShaders();

    if (defaultShader == null)
    {
      Plugin.Logger.LogError("Sprites/Default shader not found");
      return false;
    }

    if (litShader == null)
    {
      Plugin.Logger.LogError("Sprites/Lit shader not found");
      return false;
    }

    if (colorFlashShader == null)
    {
      Plugin.Logger.LogError("Sprites/Default-ColorFlash shader not found");
      return false;
    }

    Renderer[] renderers = npc.GetComponentsInChildren<Renderer>(true);
    foreach (var renderer in renderers)
    {
      Shader shader = null;

      if (renderer.name.StartsWith("pop") ||
          renderer.name == "Activate Pop" ||
          renderer.name == "Leave Pop" ||
          renderer.name == "shake" ||
          renderer.name.StartsWith("fung_immediate_BG"))
      {
        shader = defaultShader;
      }
      else if (renderer.name.StartsWith("Ambient shrooms") ||
          renderer.name.StartsWith("Dust") ||
          renderer.name.StartsWith("Pt Blastoff") ||
          renderer.name == "Pt Trail")
      {
        shader = litShader;
      }
      else if (renderer.name == "Mr_Mushroom_Needolin_Appear" ||
          renderer.name.StartsWith("Mr Mushroom NPC"))
      {
        shader = colorFlashShader;
      }

      if (shader == null)
        continue;

      renderer.material.shader = shader;
      Plugin.Logger.LogDebug($"Fixed shader for: {renderer.name} renderer");
    }

    tk2dSprite[] tk2dSprites = npc.GetComponentsInChildren<tk2dSprite>();
    foreach (var tk2dSprite in tk2dSprites)
    {
      if (!tk2dSprite.name.StartsWith("Mr Mushroom NPC"))
        continue;

      tk2dSprite.CurrentSprite.material.shader = colorFlashShader;
      Plugin.Logger.LogDebug($"Fixed shader for: {tk2dSprite.name} tk2dSprite");
    }

    var fsm = npc.GetComponent<PlayMakerFSM>();
    if (fsm == null)
    {
      Plugin.Logger.LogError("PlayMakerFSM component not found on Mr Mushroom");
      return false;
    }

    var spawnObjectFromGlobalPool = fsm.GetAction<SpawnObjectFromGlobalPool>("Idle Hit", 5);
    var strikePrefab = spawnObjectFromGlobalPool.gameObject.Value;
    if (strikePrefab == null)
    {
      Plugin.Logger.LogError("Strike prefab not found");
      return false;
    }

    var strikeTk2dSprite = strikePrefab.GetComponent<tk2dSprite>();
    if (strikeTk2dSprite == null)
    {
      Plugin.Logger.LogError("tk2dSprite component not found on Strike prefab");
      return false;
    }

    strikeTk2dSprite.CurrentSprite.material.shader = colorFlashShader;
    Plugin.Logger.LogDebug($"Fixed shader for: {strikeTk2dSprite.name} tk2dSprite");

    return true;
  }

  private static (Shader, Shader, Shader) FindShaders()
  {
    Shader defaultShader = null;
    Shader litShader = null;
    Shader colorFlashShader = null;
    Shader[] allShaders = Resources.FindObjectsOfTypeAll<Shader>();

    foreach (var shader in allShaders)
    {
      if (shader.name == "Sprites/Default" && defaultShader == null)
      {
        defaultShader = shader;
      }
      else if (shader.name == "Sprites/Lit" && litShader == null)
      {
        litShader = shader;
      }
      else if (shader.name == "Sprites/Default-ColorFlash" && colorFlashShader == null)
      {
        colorFlashShader = shader;
      }
      if (litShader != null && colorFlashShader != null)
      {
        break;
      }
    }

    return (defaultShader, litShader, colorFlashShader);
  }
}
