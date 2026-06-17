using System.Collections;
using UnityEngine;
using UnityEngine.UI; 


public class BossEnemy : Enemy
{
    [SerializeField] GameObject HPBarBoss, door;
    [SerializeField] AudioSource BossSound;
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


    protected virtual void AnimationDieBoss()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("isDie");
        }

    }
 

    protected virtual void DestroyEnemyBoss()
    {
        HPBarBoss.SetActive(false);
        BossSound.Stop();
        Destroy(gameObject);
    }


}
