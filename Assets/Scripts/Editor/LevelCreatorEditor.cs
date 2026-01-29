using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
public class LevelCreatorEditor : EditorWindow
{
    public List<GameObject> objectPrefabs = new List<GameObject>();
    private string savePath = "Assets/Resources/Levels/";

    [MenuItem("Tools/Hole Game/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow<LevelCreatorEditor>("Level Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Advanced Level Generator v2", EditorStyles.boldLabel);
        
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("objectPrefabs");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Generate All 48 Levels (Min 3 Groups)"))
        {
            GenerateLevels();
        }
    }

    private void GenerateLevels()
    {
        if (objectPrefabs.Count == 0)
        {
            Debug.LogError("Chưa bỏ Prefab nào vào list!");
            return;
        }

        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        for (int i = 1; i <= 48; i++)
        {
            CreateSmartLevel(i);
        }

        AssetDatabase.Refresh();
        Debug.Log("Đã tạo xong 48 levels (Mỗi level tối thiểu 3 group)!");
    }

    private void CreateSmartLevel(int levelIndex)
    {
        LevelData data = new LevelData();
        data.levelIndex = levelIndex;
        data.levelType = "MIXED";
        data.moveLimit = 30 + levelIndex; 
        data.groups = new List<GroupData>();

        // YÊU CẦU: Tối thiểu 3 Group cho mọi level
        int groupCount = Random.Range(3, 6); // Level nào cũng có từ 3 đến 5 group

        int totalScoreAvailable = 0;

        for (int g = 0; g < groupCount; g++)
        {
            GroupData group = new GroupData();
            group.groupName = "Group_" + g;
            group.objects = new List<LevelObject>(); // Danh sách chứa cả vị trí và loại prefab

            string[] shapes = { "SQUARE", "CIRCLE", "TRIANGLE", "DIAMOND" };
            string shape = shapes[Random.Range(0, shapes.Length)];
            group.shapeType = shape;

            // TÍNH TOÁN OFFSET ĐỂ CĂN GIỮA MAP
            // Thay vì xếp 0, 20, 40... (lệch phải), ta xếp -15, 0, 15 (cân bằng)
            float spacingX = 15f; // Khoảng cách giữa các group
            float totalWidth = (groupCount - 1) * spacingX;
            float startX = -totalWidth / 2f;
            
            Vector3 groupOffset = new Vector3(startX + (g * spacingX), 0, 0); 

            // Sinh vị trí
            List<Vector3> shapePositions = GenerateShapePositions(shape, 5 + levelIndex);
            
            foreach (var pos in shapePositions)
            {
                LevelObject obj = new LevelObject();
                obj.position = pos + groupOffset;
                
                // RANDOM PREFAB CHO TỪNG VẬT THỂ
                obj.prefabIndex = Random.Range(0, objectPrefabs.Count);
                
                group.objects.Add(obj);
                totalScoreAvailable += 10;
            }

            data.groups.Add(group);
        }

        data.targetScore = Mathf.CeilToInt(totalScoreAvailable * 0.75f);
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath + "Level_" + levelIndex + ".json", json);
    }
    
    // (Hàm GenerateShapePositions giữ nguyên như cũ, không đổi)
    private List<Vector3> GenerateShapePositions(string shape, int count)
    {
        List<Vector3> positions = new List<Vector3>();
        float spacing = 1.5f;

        switch (shape)
        {
            case "SQUARE":
                int side = Mathf.CeilToInt(Mathf.Sqrt(count));
                for (int x = 0; x < side; x++)
                {
                    for (int z = 0; z < side; z++)
                    {
                        if (positions.Count >= count) break;
                        float xPos = (x - side / 2f) * spacing;
                        float zPos = (z - side / 2f) * spacing;
                        positions.Add(new Vector3(xPos, 0.5f, zPos));
                    }
                }
                break;

            case "CIRCLE":
                float radius = spacing * (count / 5f);
                if (radius < 2f) radius = 2f;
                for (int i = 0; i < count; i++)
                {
                    float angle = i * (360f / count) * Mathf.Deg2Rad;
                    float xPos = Mathf.Cos(angle) * radius;
                    float zPos = Mathf.Sin(angle) * radius;
                    positions.Add(new Vector3(xPos, 0.5f, zPos));
                }
                positions.Add(new Vector3(0, 0.5f, 0)); 
                break;

            case "TRIANGLE":
                int rows = Mathf.CeilToInt(Mathf.Sqrt(count * 2));
                int currentItem = 0;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c <= r; c++)
                    {
                        if (currentItem >= count) break;
                        float xPos = (c - r / 2f) * spacing;
                        float zPos = -r * spacing;
                        positions.Add(new Vector3(xPos, 0.5f, zPos));
                        currentItem++;
                    }
                }
                break;

            case "DIAMOND":
                int dSide = Mathf.CeilToInt(Mathf.Sqrt(count));
                for (int x = -dSide; x <= dSide; x++)
                {
                    for (int z = -dSide; z <= dSide; z++)
                    {
                        if (Mathf.Abs(x) + Mathf.Abs(z) <= dSide / 2 + 1)
                        {
                            if (positions.Count >= count) break;
                            positions.Add(new Vector3(x * spacing, 0.5f, z * spacing));
                        }
                    }
                }
                break;
        }
        return positions;
    }
}
#endif
