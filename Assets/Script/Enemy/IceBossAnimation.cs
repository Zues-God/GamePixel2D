using UnityEngine;

public class IceBossAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Transform playerTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Tìm Player trong Scene
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (animator == null) return;

        // ƯU TIÊN 1: Nếu Boss đang di chuyển, lấy hướng từ vận tốc Rigidbody
        if (rb != null && rb.linearVelocity.magnitude > 0.1f)
        {
            Vector2 moveDirection = rb.linearVelocity.normalized;
            SetAnimationDirection(moveDirection.x, moveDirection.y);
        }
        // ƯU TIÊN 2: Nếu Boss đứng yên (vận tốc bằng 0), tự động quay mặt về phía Player
        else if (playerTransform != null)
        {
            Vector2 lookDirection = (playerTransform.position - transform.position).normalized;
            SetAnimationDirection(lookDirection.x, lookDirection.y);
        }
    }

    private void SetAnimationDirection(float x, float y)
    {
        // Tách bạch rõ ràng hướng nào mạnh hơn để ép về 4 hướng tuyệt đối (Up, Down, Left, Right)
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            animator.SetFloat("MoveX", x > 0 ? 1f : -1f);
            animator.SetFloat("MoveY", 0f);
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", y > 0 ? 1f : -1f);
        }
    }
}