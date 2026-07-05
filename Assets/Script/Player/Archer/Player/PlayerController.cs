using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Attack")]
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;
    public int poolSize = 15;

    private List<Arrow> arrowPool = new List<Arrow>();

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ===== CREATE POOL =====
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(arrowPrefab);
            Arrow arrow = obj.GetComponent<Arrow>();

            obj.SetActive(false);
            arrowPool.Add(arrow);
        }
    }

    void Update()
    {
        if (isDead) return;

        // ===== MOVEMENT =====
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        animator.SetBool("IsMove", movement != Vector2.zero);

        // ===== ATTACK =====
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            ShootArrow();
        }

        // ===== TEST DEATH =====
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        rb.linearVelocity = movement * moveSpeed;
    }

    // ===== GET ARROW FROM POOL =====
    Arrow GetArrowFromPool()
    {
        foreach (Arrow arrow in arrowPool)
        {
            if (!arrow.gameObject.activeInHierarchy)
                return arrow;
        }

        return null; // hết pool
    }

    // ===== SHOOT =====
    void ShootArrow()
    {
        if (firePoint == null || arrowPrefab == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

        Arrow arrow = GetArrowFromPool();

        if (arrow == null) return; // hết arrow

        arrow.transform.position = firePoint.position;
        arrow.SetAttacker(transform);
        arrow.Shoot(direction, arrowSpeed);
    }

    // ===== DEATH =====
    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Death");
    }
}