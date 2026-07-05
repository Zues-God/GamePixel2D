using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private Transform defaultPoint;
    [SerializeField] private float defaultSize = 12.5f;

    public float moveSpeed = 5;
    public float zoomSpeed = 5;

    private Vector3 targetPos;
    private float targetSize;

    void Start()
    {
        targetPos = transform.position;
        targetSize = cam.orthographicSize;
    }

    void Update()
    {
        transform.position =
            Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * moveSpeed);

        cam.orthographicSize =
            Mathf.Lerp(
                cam.orthographicSize,
                targetSize,
                Time.deltaTime * zoomSpeed);
    }

    public void Focus(Transform point, float size)
    {
        targetPos = new Vector3(
            point.position.x,
            point.position.y,
            transform.position.z);

        targetSize = size;
    }

    public void BackToLobby()
    {
        Focus(defaultPoint, defaultSize);
    }
}