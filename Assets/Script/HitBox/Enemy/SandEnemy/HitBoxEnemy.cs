using UnityEngine;

public class HitBoxEnemy : MonoBehaviour
{
    
    public float damage = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

        }
        
    }
}
