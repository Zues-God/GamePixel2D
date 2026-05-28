using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{


    [SerializeField] protected float enemyMoveSpeed = 10f;
    [SerializeField] protected float maxHP = 50f;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage = 1f;
    [SerializeField] protected float stayDamage = 1f;
    [SerializeField] private GameObject dieEffect;
    [SerializeField] GameObject bossHPBar;
    [SerializeField] GameObject door;
    public float knockBackForce = 5f;
    protected Player player;
    protected float currenHP;
    private Rigidbody2D rb;
    protected bool isKnockBack = false;
    public float knockBackTime = 0.15f;
    public Animator animator;
    protected bool isDead = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player>();
        currenHP = maxHP;
        UpdateHP();
    }
    protected virtual void Update()
    {

        if (isDead || isKnockBack) return; 
        MoveToPlayer();
    
    }
    protected void MoveToPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * enemyMoveSpeed;
            //FlipEnemy();
        }
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public virtual void TakeDamage(float damage, Transform attacker)
    {
        currenHP -= damage;
        currenHP = Mathf.Max(currenHP, 0);
        UpdateHP();
        Vector2 direction = (transform.position - attacker.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine());
        if (currenHP <= 0)
        {
            AnimationDie();
        }
    }

    IEnumerator KnockbackRoutine()
    {
        isKnockBack = true;

        yield return new WaitForSeconds(knockBackTime);

        isKnockBack = false;
    }

    protected virtual void AnimationDie()
    {
        isDead = true;   
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Animator animator = GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetTrigger("isDie");
        }

    }

    protected virtual void DestroyEnemy()
    {
        if (dieEffect != null)
        {
            Instantiate(dieEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    protected void UpdateHP()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currenHP / maxHP;
        }
    }
}

