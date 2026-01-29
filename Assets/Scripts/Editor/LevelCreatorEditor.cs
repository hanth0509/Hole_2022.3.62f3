using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
public class LevelCreatorEditor : EditorWindow
{
    // Danh sách các vật thể để spawn 
    public List<GameObject> objectPrefabs = new List<GameObject>();
    
    // Đường dẫn lưu file
    private string savePath = "Assets/Resources/Levels/";

    [MenuItem("Tools/Hole Game/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow<LevelCreatorEditor>("Level Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto Generate 48 Levels", EditorStyles.boldLabel);

        // Nút thêm/xóa Prefabs
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("objectPrefabs");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Generate All 48 Levels"))
        {
            GenerateLevels();
        }
    }

    private void GenerateLevels()
    {
        if (objectPrefabs.Count == 0)
        {
            Debug.LogError("Chưa bỏ Prefab vào list");
            return;
        }

        // Tạo thư mục nếu chưa có
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        for (int i = 1; i <= 48; i++)
        {
            CreateOneLevel(i);
        }

        AssetDatabase.Refresh();
        Debug.Log("Đã tạo xong 48 levels!");
    }

    private void CreateOneLevel(int levelIndex)
    {
        LevelData data = new LevelData();
        data.levelIndex = levelIndex;
        data.levelType = "AUTO_GEN";
        data.moveLimit = 30 + (levelIndex / 2); // Tăng thời gian theo level
        data.groups = new List<GroupData>();

        GroupData group = new GroupData();
        group.groupName = "Group_" + levelIndex;
        group.positions = new List<Vector3>();

        // --- THUẬT TOÁN TẠO VỊ TRÍ ---
        // Level càng cao, càng nhiều vật thể
        int objectCount = 10 + levelIndex; 
        int totalScoreAvailable = 0;

        // Bán kính vùng spawn: tăng dần theo level
        float radius = 3f + (levelIndex * 0.1f);

        for (int j = 0; j < objectCount; j++)
        {
            // Random vị trí trong hình tròn
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            Vector3 pos = new Vector3(randomPoint.x, 0.5f, randomPoint.y); // Y=0.5 mặc định
            group.positions.Add(pos);

            // Tính điểm giả định (Giả sử mặc định mỗi món 10 điểm)
            // Nếu muốn chính xác phải lấy từ prefab, nhưng ở Editor code sẽ hơi phức tạp
            // Nên mình tạm tính trung bình là 10 điểm/món
            totalScoreAvailable += 10;
        }
        
        data.groups.Add(group);

        // --- TÍNH TARGET SCORE ---
        // Thắng khi ăn được 70% tổng điểm
        data.targetScore = Mathf.CeilToInt(totalScoreAvailable * 0.7f);

        // --- LƯU RA FILE JSON ---
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath + "Level_" + levelIndex + ".json", json);
    }
}
#endif
