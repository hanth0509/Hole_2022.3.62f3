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
        GUILayout.Label("Advanced Level Generator", EditorStyles.boldLabel);
        
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("objectPrefabs");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Generate All 48 Levels (With Shapes)"))
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
        Debug.Log("Đã tạo xong 48 levels chuẩn xịn!");
    }

    private void CreateSmartLevel(int levelIndex)
    {
        LevelData data = new LevelData();
        data.levelIndex = levelIndex;
        data.levelType = "MIXED";
        data.moveLimit = 30 + levelIndex; 
        data.groups = new List<GroupData>();

        // Càng lên level cao, càng nhiều group (tối đa 3 group)
        int groupCount = 1 + (levelIndex / 10); 
        if (groupCount > 3) groupCount = 3;

        int totalScoreAvailable = 0;

        for (int g = 0; g < groupCount; g++)
        {
            GroupData group = new GroupData();
            group.groupName = "Group_" + g;
            group.positions = new List<Vector3>();

            // Chọn hình dạng ngẫu nhiên cho group này
            string[] shapes = { "SQUARE", "CIRCLE", "TRIANGLE", "DIAMOND" };
            string shape = shapes[Random.Range(0, shapes.Length)];
            group.shapeType = shape;

            // Offset vị trí group để không đè lên nhau (Group 2 nằm xa hơn Group 1)
            Vector3 groupOffset = new Vector3(g * 15f, 0, 0); // Cách nhau 15 đơn vị trục X

            // Sinh vị trí dựa trên Shape
            List<Vector3> shapePositions = GenerateShapePositions(shape, 5 + levelIndex); // Số lượng tăng theo level
            
            foreach (var pos in shapePositions)
            {
                group.positions.Add(pos + groupOffset);
                totalScoreAvailable += 10;
            }

            data.groups.Add(group);
        }

        data.targetScore = Mathf.CeilToInt(totalScoreAvailable * 0.75f); // Thắng khi ăn 75%

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath + "Level_" + levelIndex + ".json", json);
    }

    private List<Vector3> GenerateShapePositions(string shape, int count)
    {
        List<Vector3> positions = new List<Vector3>();
        float spacing = 1.5f; // Khoảng cách giữa các vật

        switch (shape)
        {
            case "SQUARE":
                int side = Mathf.CeilToInt(Mathf.Sqrt(count));
                for (int x = 0; x < side; x++)
                {
                    for (int z = 0; z < side; z++)
                    {
                        if (positions.Count >= count) break;
                        // Căn giữa
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
                // Thêm 1 cái ở giữa
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
                        float zPos = -r * spacing; // Xếp từ đỉnh xuống
                        positions.Add(new Vector3(xPos, 0.5f, zPos));
                        currentItem++;
                    }
                }
                break;

            case "DIAMOND":
                // Vẽ hình thoi đơn giản bằng cách xoay hình vuông hoặc xếp tọa độ |x| + |z|
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
