using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private const string LAST_SCENE = "LAST_SCENE";
    private const string HAS_SAVE = "HAS_SAVE";
    private const string SELECTED_CHARACTER = "SELECTED_CHARACTER";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Không lưu Menu và Lobby
        if (scene.name == "MainMenu" ||
            scene.name == "MainLobby")
        {
            return;
        }

        SaveScene(scene.name);
    }

    private void SaveScene(string sceneName)
    {
        PlayerPrefs.SetString(LAST_SCENE, sceneName);
        PlayerPrefs.SetInt(HAS_SAVE, 1);

        // Also persist currently selected character (if any)
        if (CharacterManager.Instance != null && CharacterManager.Instance.SelectedCharacter != null)
        {
            PlayerPrefs.SetString(SELECTED_CHARACTER, CharacterManager.Instance.SelectedCharacter.characterName);
        }

        PlayerPrefs.Save();

    }

    // Public helper to save a selected character explicitly
    public void SaveSelectedCharacter(CharacterData data)
    {
        if (data == null)
            return;

        PlayerPrefs.SetString(SELECTED_CHARACTER, data.characterName);
        PlayerPrefs.Save();
    }

    // Get the saved character name (empty if none)
    public string GetSavedCharacterName()
    {
        return PlayerPrefs.GetString(SELECTED_CHARACTER, string.Empty);
    }

    public bool HasSavedCharacter()
    {
        return PlayerPrefs.HasKey(SELECTED_CHARACTER);
    }

    public void DeleteSavedCharacter()
    {
        PlayerPrefs.DeleteKey(SELECTED_CHARACTER);
        PlayerPrefs.Save();
    }

    public bool HasSave()
    {
        return PlayerPrefs.GetInt(HAS_SAVE, 0) == 1;
    }

    public string GetLastScene()
    {
        return PlayerPrefs.GetString(LAST_SCENE);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(HAS_SAVE);
        PlayerPrefs.DeleteKey(LAST_SCENE);

        PlayerPrefs.Save();

        Debug.Log("Save Deleted");
    }
}