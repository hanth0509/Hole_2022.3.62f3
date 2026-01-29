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
  // public List<Vector3> positions; // Cũ: Chỉ lưu vị trí
  public List<LevelObject> objects; // Mới: Lưu cả vị trí và loại Prefab
}

[System.Serializable]
public class LevelObject
{
    public Vector3 position;
    public int prefabIndex; // Index của loại Prefab (0: Cây, 1: Đá...)
}
