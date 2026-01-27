using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
  public int levelIndex;
  public int targetScore;
  public int moveLimit;
  public string levelType;
  public List<GroupData> groups;
}
[System.Serializable]
public class GroupData
{
  public string groupName;
  public string shapeType;
  public List<Vector3> positions;
}
