using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    public GameObject popUpGameOver;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowGameOver()
    {
        popUpGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSave();

        SceneManager.LoadScene("MainLobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
