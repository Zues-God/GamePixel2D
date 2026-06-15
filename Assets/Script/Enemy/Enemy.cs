using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{


    [SerializeField] protected float enemyMoveSpeed = 10f;
    [SerializeField] protected float maxHP = 50f;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage = 1f;
    [SerializeField] protected float stayDamage = 1f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackCooldown = 1f;
    protected float lastAttackTime = 0f;
    public float knockBackForce = 5f;
    protected Player player;
    protected float currenHP;
    protected bool isKnockBack = false;
    public float knockBackTime = 0.15f;
    public Animator animator;
    public GameObject hitBox;
    public bool isDead = false;
    protected Rigidbody2D rb;
    protected bool isActive = false;
   





    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player>();
        currenHP = maxHP;
        UpdateHP();
    }
    protected virtual void Update()
    {
       
        if ( !isActive || isDead || isKnockBack) return;
        else if (player == null)
        {
            return;
        }
            float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < attackRange)
        {

            Attack();

        }
        else
        {
            MoveToPlayer();
        }
    }
    protected void MoveToPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * enemyMoveSpeed;
            FlipEnemy();
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
        direction.y = 0.5f;
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
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(knockBackTime);
        isKnockBack = false;
        animator.SetBool("isHurt", false);
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
        
        Destroy(gameObject);
    }

    protected void UpdateHP()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currenHP / maxHP;
        }
    }

    protected void Attack()
    {
        Debug.Log("Enemy ðang attack");
        if (Time.time - lastAttackTime < attackCooldown) return;
        {
            lastAttackTime = Time.time; 
            rb.linearVelocity = Vector2.zero;  
        }
        if (animator != null)
        {
            animator.SetTrigger("isAttack");
        }
    }
    public void EnableHixBox()
    {
        Debug.Log("On Hit Box");
        hitBox.SetActive(true);

    }

    public void DisEnableHixBox()
    {
        Debug.Log("Off Hit Box");
        hitBox.SetActive(false);

    }
    public void ActivateEnemy()
    {
        isActive = true;

        if (animator != null)
        {
            animator.SetBool("isRun", true);
        }
    }

}

