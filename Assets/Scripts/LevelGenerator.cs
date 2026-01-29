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

        // Xóa hết vật thể cũ (nếu có) trước khi tạo mới
        // (Code xóa cũ ở đây nếu cần, nhưng chuyển cảnh thì tự xóa rồi)

        foreach (var group in currentLevel.groups)
        {
            // Code cũ: dùng group.positions
            // foreach (var pos in group.positions) { SpawnObject(pos); }

            // Code mới: dùng group.objects
            foreach (var objData in group.objects)
            {
                SpawnObject(objData.position, objData.prefabIndex);
            }
        }
    }

    // Cập nhật hàm Spawn nhận thêm index
    void SpawnObject(Vector3 position, int prefabIndex)
    {
        if (objectPrefabs != null && objectPrefabs.Count > 0)
        {
            // Kiểm tra index có hợp lệ không (tránh lỗi Array Out Of Bounds)
            int index = prefabIndex;
            if (index < 0 || index >= objectPrefabs.Count)
            {
                index = 0; // Fallback về 0 nếu lỗi
            }

            GameObject prefab = objectPrefabs[index];
            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
}