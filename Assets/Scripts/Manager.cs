using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance; // Singleton để dễ gọi từ nơi khác
    public LevelGenerator levelGenerator; // Tham chiếu đến máy sinh level

    public int currentScore = 0;
    public int targetScore = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Test thử: Load Level 1 ngay khi vào game
        StartGame(1);
    }

    public void StartGame(int levelIndex)
    {
        // Reset điểm số
        currentScore = 0;
        
        // Gọi máy sinh level làm việc
        if (levelGenerator != null)
        {
            levelGenerator.GenerateLevel(levelIndex);
        }
        else
        {
            Debug.LogError("Chưa gán LevelGenerator vào Manager");
        }
    }
    public void SetupLevelInfo(LevelData data)
    {
        targetScore = data.targetScore;
        Debug.Log("Mục tiêu màn này: " + targetScore);
    }
    // Cập nhật lại hàm AddScore để kiểm tra thắng
    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Điểm: " + currentScore + "/" + targetScore);
        
        if (currentScore >= targetScore)
        {
            Debug.Log("LEVEL COMPLETED!");
            // Gọi hàm Next Level hoặc hiện UI thắng ở đây
        }
    }
}