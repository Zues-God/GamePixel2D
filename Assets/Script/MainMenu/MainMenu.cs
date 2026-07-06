using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingGame;
    [SerializeField] private GameObject continuePopup;

    private void Start()
    {
        continuePopup.SetActive(false);
    }
    public void StartGame()
    {
        if (SaveManager.Instance.HasSave())
        {
            continuePopup.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MainLobby");
        }
    }
    public void ContinueGame()
    {
        string scene =
            SaveManager.Instance.GetLastScene();

        SceneManager.LoadScene(scene);
    }
    public void NewGame()
    {
        SaveManager.Instance.DeleteSave();

        SceneManager.LoadScene("MainLobby");
    }
    public void CloseContinuePopup()
    {
        continuePopup.SetActive(false);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
#endif
        Application.Quit();
    }
    public void OpenSetting()
    {
        settingGame.SetActive(true);
    }
    public void CloseSetting()
    {
        settingGame.SetActive(false);
    }
}
