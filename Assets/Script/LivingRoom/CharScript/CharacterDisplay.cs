using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{

    public CharacterData characterData;
    [SerializeField] private CameraController lobbyCamera;
    [SerializeField] private float zoomSize = 4f;
    public Transform spawnPoint;
    public Transform focusPoint;

    private void OnMouseDown()
    {
        lobbyCamera.Focus(focusPoint, zoomSize);
        SelectionManager.Instance.ShowCharacterInfo(characterData, this);
    }
}