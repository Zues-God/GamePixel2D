using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("GamePlay");
        SceneManager.LoadScene("BossRoomSand");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("GameQuit");
    }
    public void SettingGame()
    {
        SceneManager.LoadScene("SettingScene");
        Debug.Log("SettingScene");
    }
}
