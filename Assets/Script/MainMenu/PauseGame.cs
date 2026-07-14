using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseGame;

    private bool isPaused = false;

    private void Start()
    {
        pauseGame.SetActive(false); // đảm bảo lúc đầu tắt
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {

            Pause();
            Debug.LogWarning("Pause");

        }
    }

    public void Pause()
    {
        Debug.LogWarning("Pause");
        pauseGame.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseGame.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}