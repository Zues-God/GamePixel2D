using UnityEngine;
using UnityEngine.UI;

public class Wizar : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("HP")]
    [SerializeField] private float maxHpPlayer = 100f;
    [SerializeField] private Image hpBar;

    [Header("Audio")]
    [SerializeField] private AudioSource stopAudio;

    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;

    private float currentHpPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHpPlayer = maxHpPlayer;
        UpdateHP();

        Debug.Log("Player Spawned");
    }

    private void Update()
    {
        MovePlayer();
        HandleFacingDirection(); // Gọi hàm xử lý hướng mặt ở đây
    }

    private void MovePlayer()
    {
        Vector2 playerInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rb.linearVelocity = playerInput.normalized * moveSpeed;

        if (playerInput != Vector2.zero)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    // Hàm mới xử lý hướng quay mặt
    private void HandleFacingDirection()
    {
        // Trường hợp 2: Khi đang click hoặc đè chuột trái (0 là chuột trái)
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Xoay mặt theo vị trí chuột
            if (mousePos.x < transform.position.x)
            {
                rbSprite.flipX = true;
            }
            else
            {
                rbSprite.flipX = false;
            }
        }
        // Trường hợp 1: Khi KHÔNG click/đè chuột
        else
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            // Xoay mặt theo hướng di chuyển (trái/phải)
            if (horizontalInput > 0)
            {
                rbSprite.flipX = false; // Đi sang phải, mặt quay phải
            }
            else if (horizontalInput < 0)
            {
                rbSprite.flipX = true;  // Đi sang trái, mặt quay trái
            }
            // Nếu horizontalInput == 0 (không bấm nút di chuyển trái phải), nhân vật sẽ giữ nguyên hướng mặt hiện tại.
        }
    }

    public void TakeDamage(float damage)
    {
        currentHpPlayer -= damage;
        currentHpPlayer = Mathf.Max(currentHpPlayer, 0);

        UpdateHP();

        Debug.Log("Player nhận sát thương: " + damage + " | HP còn: " + currentHpPlayer);

        if (currentHpPlayer <= 0)
        {
            Die();
        }
    }

    public void Heal(float healValue)
    {
        currentHpPlayer += healValue;
        currentHpPlayer = Mathf.Min(currentHpPlayer, maxHpPlayer);

        UpdateHP();

        Debug.Log("Player hồi máu: " + healValue + " | HP hiện tại: " + currentHpPlayer);
    }

    private void Die()
    {
        Debug.Log("Player Dead");

        if (stopAudio != null)
        {
            stopAudio.Stop();
        }

        Destroy(gameObject);
    }

    private void UpdateHP()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHpPlayer / maxHpPlayer;
        }
    }
}