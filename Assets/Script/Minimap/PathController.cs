using UnityEngine;

public class PathController : MonoBehaviour
{
    [Header("UI của chính đường đi này")]
    public GameObject uiPathLine; // Kéo thanh UI dẹt tương ứng từ Map_Content vào đây

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu là Player bước vào đường đi
        if (other.CompareTag("Player"))
        {
            // Gọi bộ não tổng để kéo đường đi này vào giữa màn hình
            MinimapManager.Instance.OnPlayerEnterPath(uiPathLine);
        }
    }
}