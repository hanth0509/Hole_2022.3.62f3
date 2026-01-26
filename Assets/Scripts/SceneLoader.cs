using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    void Start()
    {
    }

    public void LoadSceneHome() {
        SceneManager.LoadScene("Home");
    }
    public void LoadSceneLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level");
    }
    public void LoadSceneGamePlay() {
        SceneManager.LoadScene("GamePlay");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
