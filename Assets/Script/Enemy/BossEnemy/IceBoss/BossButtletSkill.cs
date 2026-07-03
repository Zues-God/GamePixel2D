using UnityEngine;

public class BossButtletSkill : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Bounce")]
    [SerializeField] private bool canBounce = true;
    [SerializeField] private int maxBounce = 3;

    [Header("Bounce Fix")]
    [SerializeField] private float pushOutDistance = 0.1f;
    [SerializeField] private float bounceCooldown = 0.05f;

    private Vector2 direction;
    private float speed;
    private float damage;

    private int currentBounce = 0;
    private float nextBounceTime = 0f;

    public void Initialize(
        Vector2 dir,
        float moveSpeed,
        float bulletDamage)
    {
        direction = dir.normalized;
        speed = moveSpeed;
        damage = bulletDamage;

        RotateBullet();

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    private void RotateBullet()
    {
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Chống va chạm liên tục trong cùng một frame
        if (Time.time < nextBounceTime)
            return;

        //----------------------------
        // Player
        //----------------------------
        Player player =
            collision.collider.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);

            Destroy(gameObject);

            return;
        }

        //----------------------------
        // Wall
        //----------------------------
        if (collision.collider.CompareTag("Wall"))
        {
            if (!canBounce)
            {
                Destroy(gameObject);
                return;
            }

            currentBounce++;

            Debug.Log($"Bounce {currentBounce}/{maxBounce}");

            if (currentBounce > maxBounce)
            {
                Destroy(gameObject);
                return;
            }

            Vector2 normal =
                collision.contacts[0].normal;

            direction =
                Vector2.Reflect(direction, normal).normalized;

            // Đẩy viên đạn ra khỏi tường
            transform.position +=
                (Vector3)(direction * pushOutDistance);

            RotateBullet();

            // Chống va chạm liên tục
            nextBounceTime =
                Time.time + bounceCooldown;
        }
    }
}