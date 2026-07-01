using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    private float damage;

    public void Initialize(float scale, float damage)
    {
        transform.localScale = Vector3.one * scale;
        this.damage = damage;

        Debug.Log($"Scale: {scale}");
        Debug.Log($"Damage: {damage}");
    }

    private void Start()
    {
        Destroy(gameObject, 0.6f);
    }

    public void DealDamage()
    {
        Debug.Log("BOOM");

        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                1f);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log(
                    enemy.name +
                    " nhận " +
                    damage +
                    " damage");
            }
        }
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