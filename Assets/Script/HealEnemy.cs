using Unity.Mathematics;
using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private GameObject healDrop;
    [SerializeField] private float healValue = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           if (player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }
    protected override void Die()
    {

        if (healDrop !=  null)
        {
            GameObject heal = Instantiate(healDrop, transform.position, Quaternion.identity);
            HealPlayer();
            Destroy(heal, 5f);
        }
        base.Die();
    }
    private void HealPlayer()
    {
        if (player != null)
        {
            player.Heal(healValue);
        }
    }

}
