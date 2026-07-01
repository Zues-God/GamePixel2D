using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("Không có CharacterManager!");
            return;
        }

        CharacterData character =
            CharacterManager.Instance.SelectedCharacter;

        if (character == null)
        {
            Debug.LogError("Chưa chọn nhân vật!");
            return;
        }

        Instantiate(
            character.playerPrefab,
            spawnPoint.position,
            Quaternion.identity);

        Debug.Log("Spawn Player: " + character.name);
    }
}