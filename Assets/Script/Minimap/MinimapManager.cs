using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;

    [Header("Cấu trúc UI Tổng")]
    public RectTransform mapContent; 

    [Header("Khung đánh dấu Vị Trí (3 Khung riêng biệt)")]
    public RectTransform roomHighlight;          
    public RectTransform horizontalPathHighlight; 
    public RectTransform verticalPathHighlight;   

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (mapContent != null)
        {
            foreach (Transform child in mapContent)
            {
                child.gameObject.SetActive(false);
            }
        }

        ResetAllHighlights();
    }

    public void OnPlayerEnterRoom(RoomController currentRoom)
    {
        if (currentRoom == null || mapContent == null) return;

        SetRoomUIState(currentRoom.uiRoomSquare, Color.white);

        Vector2 roomPositionInMap = currentRoom.uiRoomSquare.GetComponent<RectTransform>().anchoredPosition;
        mapContent.anchoredPosition = -roomPositionInMap;

        ResetAllHighlights();
        if (roomHighlight != null)
        {
            roomHighlight.gameObject.SetActive(true);
            roomHighlight.position = currentRoom.uiRoomSquare.transform.position;
        }

        if (currentRoom.uiConnectedPaths != null)
        {
            foreach (GameObject path in currentRoom.uiConnectedPaths)
            {
                if (path != null) path.SetActive(true);
            }
        }

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

    public void OnPlayerEnterPath(GameObject uiPath)
    {
        if (uiPath == null || mapContent == null) return;

        SetRoomUIState(uiPath, Color.white);

        Vector2 pathPositionInMap = uiPath.GetComponent<RectTransform>().anchoredPosition;
        mapContent.anchoredPosition = -pathPositionInMap;

        ResetAllHighlights();

        RectTransform pathRect = uiPath.GetComponent<RectTransform>();

        float zAngle = pathRect.localEulerAngles.z;

        bool isRotatedToVertical = Mathf.Abs(Mathf.DeltaAngle(zAngle, 0f)) > 45f;

        if (!isRotatedToVertical)
        {
            if (horizontalPathHighlight != null)
            {
                horizontalPathHighlight.gameObject.SetActive(true);
                horizontalPathHighlight.position = uiPath.transform.position;

                horizontalPathHighlight.sizeDelta = pathRect.sizeDelta;
            }
        }
        else
        {
            if (verticalPathHighlight != null)
            {
                verticalPathHighlight.gameObject.SetActive(true);
                verticalPathHighlight.position = uiPath.transform.position;

                verticalPathHighlight.sizeDelta = new Vector2(pathRect.sizeDelta.y, pathRect.sizeDelta.x);
            }
        }
    }

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