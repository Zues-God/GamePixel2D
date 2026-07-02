using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("UI của chính phòng này")]
    public GameObject uiRoomSquare;

    [Header("UI đường đi xuất phát từ phòng này")]
    public GameObject[] uiConnectedPaths;

    [Header("UI các phòng hàng xóm (sẽ hiện mờ)")]
    public GameObject[] adjacentRoomsUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MinimapManager.Instance.OnPlayerEnterRoom(this);
        }
    }
}