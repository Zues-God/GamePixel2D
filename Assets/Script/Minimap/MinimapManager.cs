using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;
    public GameObject highlightFrame; // Kéo thả Object "Highlight_Frame" vào đây

    [Header("Cấu trúc UI")]
    public RectTransform mapContent; // Kéo thả Object cha "Map_Content" vào đây

    void Start()
    {
        // 1. Tự động duyệt qua TẤT CẢ các phòng và đường đi con bên trong Map_Content để ẩn chúng đi
        if (mapContent != null)
        {
            foreach (Transform child in mapContent)
            {
                child.gameObject.SetActive(false);
            }
        }

        // 2. Nếu bạn có cái Khung viền vàng (Highlight Frame) thì cho nó hiện lên
        if (highlightFrame != null)
        {
            highlightFrame.gameObject.SetActive(true);
        }
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnPlayerEnterRoom(RoomController currentRoom)
    {
        if (currentRoom == null || mapContent == null) return;

        // 1. SÁNG RỰC: Hiện và đổi màu phòng hiện tại thành màu gốc (Trắng/Alpha = 1)
        SetRoomUIState(currentRoom.uiRoomSquare, Color.white);

        // 2. DỊCH CHUYỂN BẢN ĐỒ: Kéo Map_Content để phòng hiện tại nhảy vào đúng tâm Minimap
        Vector2 roomPositionInMap = currentRoom.uiRoomSquare.GetComponent<RectTransform>().anchoredPosition;
        mapContent.anchoredPosition = -roomPositionInMap;

        // 3. HIỆN HÀNH LANG: Hiện các đường đi nối từ phòng này ra ngoài
        if (currentRoom.uiConnectedPaths != null)
        {
            foreach (GameObject path in currentRoom.uiConnectedPaths)
            {
                if (path != null) path.SetActive(true);
            }
        }

        // 4. HIỆN MỜ PHÒNG TIẾP THEO: Gợi ý hướng đi cho người chơi (Bóng mờ đen)
        if (currentRoom.adjacentRoomsUI != null)
        {
            foreach (GameObject nextRoomUI in currentRoom.adjacentRoomsUI)
            {
                if (nextRoomUI != null)
                {
                    // Nếu phòng hàng xóm chưa từng được mở, cho nó hiện mờ mờ
                    if (!nextRoomUI.activeSelf)
                    {
                        // Đổi màu sang xám tối và giảm độ trong suốt (Alpha = 0.5)
                        SetRoomUIState(nextRoomUI, new Color(0.2f, 0.2f, 0.2f, 0.5f));
                    }
                }
            }
        }
    }

    // Hàm mới: Kích hoạt khi Player đi vào hành lang/đường đi
    public void OnPlayerEnterPath(GameObject uiPath)
    {
        if (uiPath == null || mapContent == null) return;

        // 1. Sáng rực đường đi này lên (phòng trường hợp trước đó nó chưa được bật)
        SetRoomUIState(uiPath, Color.white);

        // 2. DỊCH CHUYỂN BẢN ĐỒ: Đưa ĐƯỜNG ĐI này vào chính giữa tâm Minimap
        Vector2 pathPositionInMap = uiPath.GetComponent<RectTransform>().anchoredPosition;
        mapContent.anchoredPosition = -pathPositionInMap;
    }

    private void SetRoomUIState(GameObject roomUI, Color color)
    {
        if (roomUI != null)
        {
            roomUI.SetActive(true);
            Image img = roomUI.GetComponent<Image>();
            if (img != null) img.color = color;
        }
    }
}