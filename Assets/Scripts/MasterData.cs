using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterData : MonoBehaviour
{
    public static MasterData instance;
    public int currentLevelIndex = 1;
    public int unlockedLevel = 1;
    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UnlockNextLevel(int nextLevel)
    {
        // Nếu level mới này chưa mở -> thì mở
        if (nextLevel > unlockedLevel)
        {
            unlockedLevel = nextLevel;
            // Ở đây sau này sẽ thêm lệnh Save game (PlayerPrefs/EasySave)
            Debug.Log("Đã mở khóa Level: " + unlockedLevel);
        }
    }
}
