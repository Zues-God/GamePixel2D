using UnityEngine;

public class LavaBoss : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 500f;
    public float damage = 30f;
    public float moveSpeed = 2f;
    public float attackRange = 2f;


    [Header("Reference")]
    public Transform player; 
    public GameObject hitBox;
    private float currentHP;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;


    private bool attacking;
    private bool dead;



    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }



    void Start()
    {
        currentHP = maxHP;

        hitBox.SetActive(false);
    }



    void Update()
    {
        if (dead)
            return;


        float distance = Vector2.Distance(
            transform.position,
            player.position
        );


        if (distance <= attackRange)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }



    void Move()
    {
        Vector2 direction =
            (player.position - transform.position).normalized;


        rb.linearVelocity = direction * moveSpeed;


        animator.SetBool("isMoving", true);


        if (direction.x < 0)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }



    void Attack()
    {
        rb.linearVelocity = Vector2.zero;


        animator.SetBool("isMoving", false);


        if (!attacking)
        {
            attacking = true;

            animator.SetTrigger("Attack");
        }
    }



    // Animation Event gọi hàm này
    public void EnableHitBox()
    {
        hitBox.SetActive(true);
    }



    // Animation Event gọi hàm này
    public void DisableHitBox()
    {
        hitBox.SetActive(false);

        attacking = false;
    }



    public void TakeDamage(float damage)
    {
        if (dead)
            return;


        currentHP -= damage;


        if (currentHP <= 0)
        {
            Die();
        }
    }



    void Die()
    {
        dead = true;

        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("Die");


        hitBox.SetActive(false);


        Destroy(gameObject, 2f);
    }
}