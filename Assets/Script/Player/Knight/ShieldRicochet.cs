using System.Collections;
using UnityEngine;

public class ShieldRicochet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;

    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float rotateSpeed = 720f;

    private Vector2 direction;
    private PlayerShield playerShield;

    private bool isFlying = false;


    public void Init(Vector2 dir, Transform playerRef)
    {
        direction = dir.normalized;

        playerShield = playerRef.GetComponent<PlayerShield>();

        transform.parent = null;

        isFlying = true;

        StartCoroutine(RicochetRoutine());
    }


    void Update()
    {
        if (isFlying)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }


    private IEnumerator RicochetRoutine()
    {
        float timer = 0f;

        while (timer < lifeTime)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, wallMask);
            if (hit.collider != null)
            {
                direction = Vector2.Reflect(direction, hit.normal);
            }

            timer += Time.deltaTime;

            yield return null;
        }

        isFlying = false;

        ReturnShieldToPlayer();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(2.5f, transform.root);

            }
        }
    }


    private void ReturnShieldToPlayer()
    {
        if (playerShield != null)
        {
            playerShield.RecoverShield();
        }
        Destroy(gameObject);
    }
}