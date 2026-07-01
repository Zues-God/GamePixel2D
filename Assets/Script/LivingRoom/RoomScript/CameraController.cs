using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float normalSize = 8f;
    [SerializeField] private float zoomSize = 5f;

    private void Awake()
    {
        Instance = this;
    }

    public void ZoomIn()
    {
        mainCamera.orthographicSize =
            zoomSize;
    }

    public void ZoomOut()
    {
        mainCamera.orthographicSize =
            normalSize;
    }
}