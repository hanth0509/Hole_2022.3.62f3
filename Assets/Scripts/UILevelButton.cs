using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Nếu dùng TextMeshPro

public class UILevelButton : MonoBehaviour
{
    public int levelIndex; // Level mà nút này sẽ mở (1, 2, 3...)
    public TextMeshProUGUI levelText; // Text hiển thị số level
    public Button myButton; // Nút bấm

    private void Start()
    {
        // 1. Hiển thị số level lên nút
        if (levelText != null)
        {
            levelText.text = levelIndex.ToString();
        }

        // 2. Gán sự kiện khi bấm nút
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnLevelSelected);
        }
        
        // 3. (Tuỳ chọn) Kiểm tra xem level này mở khóa chưa
        UpdateLockState();
    }

    void UpdateLockState()
    {
        // Lấy thông tin level đã mở khóa từ MasterData
        if (MasterData.instance != null)
        {
            if (levelIndex > MasterData.instance.unlockedLevel)
            {
                // Nếu chưa mở khóa -> Khóa nút lại
                 myButton.interactable = false;
            }
            else
            {
                 myButton.interactable = true;
            }
        }
    }

    void OnLevelSelected()
    {
        // BƯỚC QUAN TRỌNG NHẤT:
        // Lưu lại level người dùng chọn vào MasterData
        if (MasterData.instance != null)
        {
            MasterData.instance.currentLevelIndex = levelIndex;
        }

        // Chuyển sang scene Gameplay (tên Scene phải chuẩn nhé)
        SceneManager.LoadScene("GamePlay"); // Hoặc tên scene của bạn là "Level"
    }
}
