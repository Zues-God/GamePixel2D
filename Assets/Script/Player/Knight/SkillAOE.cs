using UnityEngine;

public class SkillAOE : MonoBehaviour
{
    [SerializeField] public float damage = 5f;
    [SerializeField] private Audio audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
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
