using UnityEngine;

namespace MrMushroomExtraContent;

public static class GameObjectFinder
{
  public static GameObject FindMrMushroomNpc()
  {
    var objects = Object.FindObjectsByType<NeedolinAppearTimeline>(FindObjectsInactive.Include, FindObjectsSortMode.None);

    foreach (var obj in objects)
    {
      if (obj.name == "Mr_Mushroom_Appear")
      {
        var npcTransform = obj.gameObject.transform.parent;
        if (npcTransform == null)
        {
          return null;
        }

        return npcTransform.gameObject;
      }
    }

    return null;
  }

  public static PlayerDetector FindPlayerDetector()
  {
    var objects = Object.FindObjectsByType<PlayerDetector>(FindObjectsSortMode.None);

    foreach (var obj in objects)
    {
      if (obj.name == "Close Range")
      {
        return obj;
      }
    }

    return null;
  }
}
