using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        Debug.Log("PlayerSpawner Start");

        Debug.Log(CharacterManager.Instance);

        if (CharacterManager.Instance == null)
        {
            Debug.Log("CharacterManager not found - creating runtime instance");
            var go = new GameObject("CharacterManager");
            go.AddComponent<CharacterManager>();
        }

        Debug.Log(CharacterManager.Instance.SelectedCharacter);

        CharacterData character =
            CharacterManager.Instance.SelectedCharacter;

        if (character == null)
        {
            return;
        }

        Instantiate(
            character.playerPrefab,
            spawnPoint.position,
            Quaternion.identity);
    }   
}