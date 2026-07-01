using UnityEngine;
using UnityEngine.Tilemaps;

public class Arrow : MonoBehaviour
{
    [Header("Stats")]
    public float damage = 10f;
    public float lifeTime = 3f;

    private Transform attacker;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetAttacker(Transform atk)
    {
        attacker = atk;
    }

    public void Shoot(Vector2 direction, float speed)
    {
        Debug.Log("Shoot direction = " + direction);
        Debug.Log("Speed = " + speed);

        gameObject.SetActive(true);

        rb.gravityScale = 0f;
        rb.linearVelocity = direction * speed;

        Debug.Log("Velocity = " + rb.linearVelocity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        CancelInvoke();
        Invoke(nameof(DisableArrow), lifeTime);
    }

    void DisableArrow()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage, attacker);
            }

            DisableArrow();
        }

        // ===== TILEMAP / WALL =====
        if (collision.GetComponent<TilemapCollider2D>() != null)
        {
            DisableArrow();
        }
    }
}