using UnityEngine;

public class FileBall : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] public float takeDamage = 3f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position +=
            (Vector3)direction *
            speed *
            Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(takeDamage, transform.root);
            }
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}