using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public CharacterData SelectedCharacter;
    private const string SELECTED_CHARACTER = "SELECTED_CHARACTER";
    private void Awake()
    {
        Debug.Log("CharacterManager Awake");
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            LoadCharacter();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(CharacterData data)
    {
        SelectedCharacter = data;

        PlayerPrefs.SetString(
        SELECTED_CHARACTER,
        data.characterName);

        PlayerPrefs.Save();
    }
    public void LoadCharacter()
    {
        if (!PlayerPrefs.HasKey(SELECTED_CHARACTER))
        {
            Debug.Log("Không có Character được lưu");
            return;
        }

        string characterName =
            PlayerPrefs.GetString(SELECTED_CHARACTER);

        Debug.Log("Character cần load: " + characterName);

        if (CharacterDatabase.Instance == null)
        {
            Debug.LogError("CharacterDatabase chưa được tạo!");
            return;
        }

        SelectedCharacter =
            CharacterDatabase.Instance.GetCharacter(characterName);

        Debug.Log("Selected Character = " + SelectedCharacter);
    }
}