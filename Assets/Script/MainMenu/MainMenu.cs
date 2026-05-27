using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingGame;
    public void StartGame()
    {
        Debug.Log("GamePlay");
        SceneManager.LoadScene("BossRoomSand");
    }
    public void QuitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
    #endif
        Application.Quit();
        Debug.Log("GameQuit");
    }
    public void OpenSetting()
    {
        settingGame.SetActive(true);
        Debug.Log("SettingOpen");
    }
    public void CloseSetting()
    {
        settingGame.SetActive(false);
        Debug.Log("SettingClose");
    }
}
