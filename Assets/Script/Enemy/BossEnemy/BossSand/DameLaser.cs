using UnityEngine;

public class DameLaser : MonoBehaviour
{
    [SerializeField] private float damageLaser = 20f;

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damageLaser);
            }
        }
    }
}
