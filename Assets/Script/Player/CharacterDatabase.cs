using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance;

    [SerializeField]
    private CharacterData[] characters;

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

    public CharacterData GetCharacter(string characterName)
    {
        foreach (CharacterData character in characters)
        {
            if (character.characterName == characterName)
            {
                return character;
            }
        }

        return null;
    }
}