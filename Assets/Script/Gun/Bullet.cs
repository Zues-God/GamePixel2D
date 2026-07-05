using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveBulletSpeed = 30f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField]  GameObject bloodPrefab;


    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        MoveBullet();

    }

    private void MoveBullet()
    {
        transform.Translate(Vector2.right * moveBulletSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.root);
                GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
                Destroy(blood, 1f);
            }
            Destroy(gameObject);
        }
    }
}

