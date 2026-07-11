using UnityEngine;

public class LaserBulletGun : MonoBehaviour
{
    [Header("Laser Parts")]
    public Transform head;
    public Transform body;
    public Transform tail;

    [Header("Settings")]
    public LayerMask hitLayer;
    public float maxDistance = 50f;

    [Header("Optional")]
    public SpriteRenderer bodyRenderer;
    public BoxCollider2D bodyCollider;

    private float baseWidth = 1f;

    void Start()
    {
        if (bodyRenderer != null && bodyRenderer.sprite != null)
        {
            baseWidth = bodyRenderer.sprite.bounds.size.x;
            if (baseWidth <= 0f) baseWidth = 1f;
        }
    }

    void Update()
    {

        ShootLaser();


    }

    void ShootLaser()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector2 direction = (mouseWorld - head.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(head.position, direction, Mathf.Infinity, hitLayer);
        float length = hit.collider != null ? hit.distance : maxDistance;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        head.position = transform.position;

        UpdateLaser(length);
    }

    void UpdateLaser(float length)
    {
        float safeLength = Mathf.Max(length, 0.05f);

        if (bodyRenderer != null)
        {
            Vector3 scale = body.localScale;
            scale.x = safeLength / baseWidth;
            body.localScale = scale;
        }

        body.localPosition = Vector3.zero;
        tail.localPosition = new Vector3(safeLength, 0, 0);

        if (bodyCollider != null)
        {
            bodyCollider.size = new Vector2(safeLength, bodyCollider.size.y);
            bodyCollider.offset = new Vector2(safeLength * 0.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(100, transform.root);
            }
        }
    }
}