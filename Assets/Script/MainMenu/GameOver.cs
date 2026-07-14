using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject popUpGameOver;

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
