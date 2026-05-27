using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;

    [Header("Cấu trúc UI Tổng")]
    public RectTransform mapContent; // Kéo thả Object cha "Map_Content" vào đây

    [Header("Khung đánh dấu Vị Trí (3 Khung riêng biệt)")]
    public RectTransform roomHighlight;           // Khung dành riêng cho PHÒNG (Hình vuông)
    public RectTransform horizontalPathHighlight; // Khung dành riêng cho ĐƯỜNG NGANG
    public RectTransform verticalPathHighlight;   // Khung dành riêng cho ĐƯỜNG DỌC

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 1. Tự động ẩn toàn bộ phòng và đường đi lúc đầu game
        if (mapContent != null)
        {
            foreach (Transform child in mapContent)
            {
                child.gameObject.SetActive(false);
            }
        }

        // 2. Ẩn tất cả các khung highlight lúc mới vào game
        ResetAllHighlights();
    }

    // HÀM XỬ LÝ KHI PLAYER VÀO PHÒNG
    public void OnPlayerEnterRoom(RoomController currentRoom)
    {
        if (currentRoom == null || mapContent == null) return;

        SetRoomUIState(currentRoom.uiRoomSquare, Color.white);

        // Dịch chuyển bản đồ đưa phòng vào tâm
        Vector2 roomPositionInMap = currentRoom.uiRoomSquare.GetComponent<RectTransform>().anchoredPosition;
        mapContent.anchoredPosition = -roomPositionInMap;

        // QUẢN LÝ HIGHLIGHT: Tắt hết khung đường, bật khung phòng
        ResetAllHighlights();
        if (roomHighlight != null)
        {
            roomHighlight.gameObject.SetActive(true);
            roomHighlight.position = currentRoom.uiRoomSquare.transform.position;
        }

        // Hiện các hành lang nối tiếp
        if (currentRoom.uiConnectedPaths != null)
        {
            foreach (GameObject path in currentRoom.uiConnectedPaths)
            {
                if (path != null) path.SetActive(true);
            }
        }

        // Hiện mờ phòng kế cạnh
        if (currentRoom.adjacentRoomsUI != null)
        {
            foreach (GameObject nextRoomUI in currentRoom.adjacentRoomsUI)
            {
                if (nextRoomUI != null && !nextRoomUI.activeSelf)
                {
                    SetRoomUIState(nextRoomUI, new Color(0.2f, 0.2f, 0.2f, 0.5f));
                }
            }
        }
    }

    // HÀM XỬ LÝ KHI PLAYER VÀO ĐƯỜNG ĐI (Kiểm tra bằng góc xoay Z)
    public void OnPlayerEnterPath(GameObject uiPath)
    {
        if (uiPath == null || mapContent == null) return;

        SetRoomUIState(uiPath, Color.white);

        // Dịch chuyển bản đồ đưa đường đi vào tâm
        Vector2 pathPositionInMap = uiPath.GetComponent<RectTransform>().anchoredPosition;
        mapContent.anchoredPosition = -pathPositionInMap;

        // Tắt hết các khung cũ để chuẩn bị bật khung mới
        ResetAllHighlights();

        RectTransform pathRect = uiPath.GetComponent<RectTransform>();

        // 🔥 CẢI TIẾN: Lấy góc xoay trục Z của đường đi UI
        float zAngle = pathRect.localEulerAngles.z;

        // Dùng DeltaAngle để tính độ lệch so với góc 0. 
        // Nếu lệch một góc lớn hơn 45 độ (ví dụ bạn xoay 90 độ) -> Đây là Đường Dọc
        bool isRotatedToVertical = Mathf.Abs(Mathf.DeltaAngle(zAngle, 0f)) > 45f;

        if (!isRotatedToVertical)
        {
            // 1. ĐƯỜNG NGANG (Góc quay gần 0 hoặc 180 độ)
            if (horizontalPathHighlight != null)
            {
                horizontalPathHighlight.gameObject.SetActive(true);
                horizontalPathHighlight.position = uiPath.transform.position;

                // Giữ nguyên kích thước 40x5 của đường ngang
                horizontalPathHighlight.sizeDelta = pathRect.sizeDelta;
            }
        }
        else
        {
            // 2. ĐƯỜNG DỌC (Góc quay gần 90 hoặc 270 độ)
            if (verticalPathHighlight != null)
            {
                verticalPathHighlight.gameObject.SetActive(true);
                verticalPathHighlight.position = uiPath.transform.position;

                // MẸO: Vì thanh UI gốc của bạn có size là (40, 5) nhưng bị xoay đứng lên,
                // nên khung dọc (natively vertical) cần phải ĐẢO NGƯỢC X và Y lại thành (5, 40) để ôm khít!
                verticalPathHighlight.sizeDelta = new Vector2(pathRect.sizeDelta.y, pathRect.sizeDelta.x);
            }
        }
    }

    // Hàm phụ trợ ẩn nhanh tất cả các khung highlight
    private void ResetAllHighlights()
    {
        if (roomHighlight != null) roomHighlight.gameObject.SetActive(false);
        if (horizontalPathHighlight != null) horizontalPathHighlight.gameObject.SetActive(false);
        if (verticalPathHighlight != null) verticalPathHighlight.gameObject.SetActive(false);
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