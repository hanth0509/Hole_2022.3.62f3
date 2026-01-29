using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance; // Singleton để dễ gọi từ nơi khác
    public LevelGenerator levelGenerator; // Tham chiếu đến máy sinh level
    public Hole holeScript;
    public int currentScore = 0;
    public int targetScore = 0;
    private int nextGrowthThreshold = 50; 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    int levelToPlay = 2;
    if (MasterData.instance != null)
    {
        levelToPlay = MasterData.instance.currentLevelIndex;
    }
    StartGame(levelToPlay);
    }

    public void StartGame(int levelIndex)
    {
        // Reset điểm số
        currentScore = 0;
        nextGrowthThreshold = 50;
        
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
         if (currentScore >= nextGrowthThreshold)
        {
            // Tăng mốc tiếp theo lên (50 -> 100 -> 150...)
            nextGrowthThreshold += 50;
            
            // Gọi Hole to lên
            if (holeScript != null)
            {
                holeScript.ScaleUp();
            }
        }
        if (currentScore >= targetScore)
        {
            Debug.Log("LEVEL COMPLETED!");
            
            if (MasterData.instance != null)
            {
                // Mở khóa level tiếp theo
                MasterData.instance.UnlockNextLevel(MasterData.instance.currentLevelIndex + 1);
            }
             // Hiện UI thắng / Về lại Home
        }
    }
}