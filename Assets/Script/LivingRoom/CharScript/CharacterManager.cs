using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public CharacterData SelectedCharacter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            Debug.Log("CharacterManager Created");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(CharacterData data)
    {
        SelectedCharacter = data;

        Debug.Log("Selected Character: " + data.characterName);
    }
}