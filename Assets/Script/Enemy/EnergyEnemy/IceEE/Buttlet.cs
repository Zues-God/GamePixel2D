using UnityEngine;

public class Buttlet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float damage = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}