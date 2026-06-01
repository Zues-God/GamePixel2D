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

    protected override void AnimationDie()
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

    protected override void DestroyEnemy()
    {
        HPBarBoss.SetActive(false);
        BossSound.Stop();
        door.SetActive(false);
        Destroy(gameObject);
    }


}
