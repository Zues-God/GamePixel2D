using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (CharacterManager.Instance == null)
        {
            return;
        }

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