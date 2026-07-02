using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{

    public CharacterData characterData;

    public Transform spawnPoint;
    public Transform focusPoint;

    private void OnMouseDown()
    {
        SelectionManager.Instance.ShowCharacterInfo(characterData, this);
    }
}