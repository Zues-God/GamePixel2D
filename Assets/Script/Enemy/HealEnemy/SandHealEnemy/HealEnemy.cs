using Unity.Mathematics;
using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private GameObject healDrop;
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
    protected override void DestroyEnemy()
    {

        if (healDrop !=  null)
        {
            GameObject heal = Instantiate(healDrop, transform.position, Quaternion.identity);
            Destroy(heal, 5f);
        }
        base.DestroyEnemy();
    }

}
