using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseGame;

    public bool isPaused = false;


    public void ResumeGame()
    {
        pauseGame.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}