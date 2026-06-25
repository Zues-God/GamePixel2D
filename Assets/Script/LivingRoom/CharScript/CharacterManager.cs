using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public CharacterData SelectedCharacter;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectCharacter(
        CharacterData data)
    {
        SelectedCharacter = data;

        Debug.Log(
            "Selected: "
            + data.characterName);
    }
}