namespace MrMushroomExtraContent;

public static class GameTestingHelper
{
  public static void ResetMossMother()
  {
    var pd = PlayerData.instance;
    pd.encounteredMossMother = false;
    pd.defeatedMossMother = false;
  }

  public static void ResetBellBeast()
  {
    var pd = PlayerData.instance;
    pd.encounteredBellBeast = false;
    pd.defeatedBellBeast = false;
  }

  public static void ResetAnt02Guard()
  {
    SceneData.instance.PersistentBools.SetValue(new PersistentItemData<bool>
    {
      SceneName = "Ant_02",
      ID = "Battle Scene",
      Value = false
    });
  }
}
