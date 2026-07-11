using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    private float damage;

    public void Initialize(float scale, float damage)
    {
        transform.localScale = Vector3.one * scale;
        this.damage = damage;
    }

    private void Start()
    {
        Destroy(gameObject, 0.6f);
    }

    public void DealDamage()
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                1f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.root);
            }
        }
    }
}