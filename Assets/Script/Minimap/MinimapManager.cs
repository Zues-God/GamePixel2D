using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    // Singleton để các script khác có thể gọi:
    // MinimapManager.Instance.OnPlayerEnterRoom(...)
    public static MinimapManager Instance;

    [Header("Cấu trúc UI Tổng")]

    // Object cha chứa toàn bộ minimap
    // Ví dụ:
    // Minimap
    //    ├── Room1
    //    ├── Room2
    //    ├── Path1
    //    └── Path2
    public RectTransform mapContent;

    [Header("Khung đánh dấu Vị Trí (3 Khung riêng biệt)")]

    // Khung vàng khi Player đứng trong phòng
    public RectTransform roomHighlight;

    // Khung vàng khi Player đứng trên đường ngang
    public RectTransform horizontalPathHighlight;

    // Khung vàng khi Player đứng trên đường dọc
    public RectTransform verticalPathHighlight;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Khi bắt đầu game sẽ ẩn toàn bộ minimap
        // Người chơi sẽ khám phá dần từng phòng
        if (mapContent != null)
        {
            foreach (Transform child in mapContent)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Ẩn toàn bộ khung Highlight
        ResetAllHighlights();
    }

    //==========================================================
    // Được gọi khi Player bước vào một phòng
    //==========================================================
    public void OnPlayerEnterRoom(RoomController currentRoom)
    {
        // Thiếu dữ liệu thì bỏ qua
        if (currentRoom == null || mapContent == null)
            return;

        // Hiện phòng hiện tại trên minimap
        // Đồng thời đổi màu thành trắng
        SetRoomUIState(currentRoom.uiRoomSquare, Color.white);

        // Lấy vị trí của phòng trên minimap
        Vector2 roomPositionInMap =
            currentRoom.uiRoomSquare
            .GetComponent<RectTransform>()
            .anchoredPosition;

        // Di chuyển cả minimap
        // để phòng hiện tại luôn nằm giữa khung nhìn
        mapContent.anchoredPosition = -roomPositionInMap;

        // Ẩn các Highlight cũ
        ResetAllHighlights();

        // Hiện Highlight của phòng hiện tại
        if (roomHighlight != null)
        {
            roomHighlight.gameObject.SetActive(true);

            // Đặt Highlight đúng vị trí phòng
            roomHighlight.position =
                currentRoom.uiRoomSquare.transform.position;
        }

        //======================================================
        // Hiện các đường nối với phòng hiện tại
        //======================================================
        if (currentRoom.uiConnectedPaths != null)
        {
            foreach (GameObject path in currentRoom.uiConnectedPaths)
            {
                if (path != null)
                    path.SetActive(true);
            }
        }

        //======================================================
        // Hiện các phòng kế bên nhưng tô màu xám
        // để báo người chơi biết còn phòng chưa khám phá
        //======================================================
        if (currentRoom.adjacentRoomsUI != null)
        {
            foreach (GameObject nextRoomUI in currentRoom.adjacentRoomsUI)
            {
                if (nextRoomUI != null && !nextRoomUI.activeSelf)
                {
                    SetRoomUIState(
                        nextRoomUI,
                        new Color(0.2f, 0.2f, 0.2f, 0.5f));
                }
            }
        }
    }

    //==========================================================
    // Được gọi khi Player bước vào hành lang
    //==========================================================
    public void OnPlayerEnterPath(GameObject uiPath)
    {
        if (uiPath == null || mapContent == null)
            return;

        // Hiện hành lang
        // và đổi sang màu trắng
        SetRoomUIState(uiPath, Color.white);

        // Lấy vị trí của hành lang trên minimap
        Vector2 pathPositionInMap =
            uiPath.GetComponent<RectTransform>().anchoredPosition;

        // Di chuyển minimap
        mapContent.anchoredPosition = -pathPositionInMap;

        // Tắt Highlight cũ
        ResetAllHighlights();

        RectTransform pathRect =
            uiPath.GetComponent<RectTransform>();

        // Góc xoay của hành lang
        float zAngle = pathRect.localEulerAngles.z;

        // Nếu góc gần 90 độ
        // thì coi là đường dọc
        bool isRotatedToVertical =
            Mathf.Abs(Mathf.DeltaAngle(zAngle, 0f)) > 45f;

        //------------------------------------------------------
        // Đường ngang
        //------------------------------------------------------
        if (!isRotatedToVertical)
        {
            if (horizontalPathHighlight != null)
            {
                horizontalPathHighlight.gameObject.SetActive(true);

                horizontalPathHighlight.position =
                    uiPath.transform.position;

                // Chỉnh kích thước Highlight
                horizontalPathHighlight.sizeDelta =
                    pathRect.sizeDelta;
            }
        }

        //------------------------------------------------------
        // Đường dọc
        //------------------------------------------------------
        else
        {
            if (verticalPathHighlight != null)
            {
                verticalPathHighlight.gameObject.SetActive(true);

                verticalPathHighlight.position =
                    uiPath.transform.position;

                // Vì đường dọc nên đảo Width/Height
                verticalPathHighlight.sizeDelta =
                    new Vector2(
                        pathRect.sizeDelta.y,
                        pathRect.sizeDelta.x);
            }
        }
    }

    //==========================================================
    // Ẩn toàn bộ Highlight
    //==========================================================
    private void ResetAllHighlights()
    {
        if (roomHighlight != null)
            roomHighlight.gameObject.SetActive(false);

        if (horizontalPathHighlight != null)
            horizontalPathHighlight.gameObject.SetActive(false);

        if (verticalPathHighlight != null)
            verticalPathHighlight.gameObject.SetActive(false);
    }

    //==========================================================
    // Hiện object trên minimap
    // và đổi màu của nó
    //==========================================================
    private void SetRoomUIState(GameObject roomUI, Color color)
    {
        if (roomUI != null)
        {
            // Hiện object
            roomUI.SetActive(true);

            // Đổi màu Image
            Image img = roomUI.GetComponent<Image>();

            if (img != null)
                img.color = color;
        }
    }
}