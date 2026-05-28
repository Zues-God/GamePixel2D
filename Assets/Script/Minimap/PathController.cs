using UnityEngine;

public class PathController : MonoBehaviour
{
    [Header("UI của chính đường đi này")]
    public GameObject uiPathLine; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MinimapManager.Instance.OnPlayerEnterPath(uiPathLine);
        }
    }
}