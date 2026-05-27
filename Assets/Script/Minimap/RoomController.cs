using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("UI của chính phòng này")]
    public GameObject uiRoomSquare;       // Kéo ô vuông UI tương ứng vào đây

    [Header("UI đường đi xuất phát từ phòng này")]
    public GameObject[] uiConnectedPaths; // Kéo các đường hành lang đi ra vào đây

    [Header("UI các phòng hàng xóm (sẽ hiện mờ)")]
    public GameObject[] adjacentRoomsUI;  // Kéo các ô vuông UI của các phòng kế cạnh vào đây

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Đảm bảo Player của bạn có Tag là "Player"
        if (other.CompareTag("Player"))
        {
            // Kích hoạt logic cập nhật Minimap
            MinimapManager.Instance.OnPlayerEnterRoom(this);
        }
    }
}