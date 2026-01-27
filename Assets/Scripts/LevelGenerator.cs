using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> objectPrefabs; 

    public void GenerateLevel(int levelIndex)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Levels/Level_" + levelIndex);
        
        if (jsonFile == null)
        {
            Debug.LogError("Không tìm thấy file level: " + levelIndex);
            return;
        }

        LevelData currentLevel = JsonUtility.FromJson<LevelData>(jsonFile.text);
        if (Manager.instance != null)
        {
            Manager.instance.SetupLevelInfo(currentLevel);
        }
        
        foreach (var group in currentLevel.groups)
        {
            foreach (var pos in group.positions)
            {
                SpawnObject(pos);
            }
        }
    }

    void SpawnObject(Vector3 position)
    {
        if (objectPrefabs.Count > 0)
        {
            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Count)];
            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
}