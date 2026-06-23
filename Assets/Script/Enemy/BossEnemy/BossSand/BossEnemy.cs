using System.Collections;
using UnityEngine;
using UnityEngine.UI; 


public class BossEnemy : Enemy
{
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
                Debug.Log("Player take damage: " + enterDamage);
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
                Debug.Log("Player take damage: " + stayDamage);

            }
        }
    }


}
