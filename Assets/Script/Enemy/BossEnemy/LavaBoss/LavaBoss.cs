using UnityEngine;
using UnityEngine.UI;

public class LavaBoss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 1000f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Skill")]
    [SerializeField] private float skillCooldown = 8f;

    [Header("UI")]
    [SerializeField] private Image hpBar;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject hitBox;

    private Player player;

    private float currentHP;
    private float lastAttackTime;
    private float lastSkillTime;

    private bool isDead;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();

        currentHP = maxHP;

        UpdateHP();
    }

    private void Update()
    {
        if (isDead) return;

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > attackRange)
        {
            MoveToPlayer();
        }
        else
        {
            Attack();
        }

        Flip();
    }

    private void MoveToPlayer()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;

        rb.linearVelocity = dir * moveSpeed;

        animator.SetBool("isRun", true);
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isRun", false);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            animator.SetTrigger("isAttack");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;

        UpdateHP();

        animator.SetTrigger("isHurt");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("isDie");
    }

    private void UpdateHP()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHP / maxHP;
        }
    }

    private void Flip()
    {
        if (player == null) return;

        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Animation Event
    public void EnableHitBox()
    {
        hitBox.SetActive(true);
    }

    // Animation Event
    public void DisableHitBox()
    {
        hitBox.SetActive(false);
    }

    // Animation Event
    public void CastSkill()
    {
        Debug.Log("Boss Cast Lava Skill");
    }

    // Animation Event
    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}