using System.Collections;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform body;
    [SerializeField] private Transform tail;
    [SerializeField] private GameObject laser;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float stickTime = 1.2f;

    [Header("Collision")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private GameObject warningLaser;
    [SerializeField] private float warningTime = 1.5f;

    private SpriteRenderer bodyRenderer;
    private BoxCollider2D bodyCollider;
    private float currentLength;
    private Vector2 lockedPosition;

    private SpriteRenderer warningRenderer;
    private float warningBaseWidth = 1f;


    private void Awake()
    {
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        bodyCollider = body.GetComponent<BoxCollider2D>();

        if (warningLaser != null)
        {
            warningRenderer = warningLaser.GetComponent<SpriteRenderer>();
            if (warningRenderer != null && warningRenderer.sprite != null)
            {
                warningBaseWidth = warningRenderer.sprite.bounds.size.x;
                if (warningBaseWidth <= 0f) warningBaseWidth = 1f;
            }
        }

        UpdateLaser(0);
    }

    public void SetTarget(Transform t)
    {
        player = t;
    }

    public IEnumerator Fire()
    {
        lockedPosition = player.position;

        Vector2 dir = (lockedPosition - (Vector2)head.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        float distanceToTarget = Vector2.Distance(head.position, lockedPosition);

        RaycastHit2D wallHit = Physics2D.Raycast(head.position, dir, 100f, wallMask);
        float lengthToWallOrTarget = wallHit.collider != null
            ? Mathf.Min(wallHit.distance, distanceToTarget)
            : distanceToTarget;

        if (warningLaser != null)
        {
            warningLaser.SetActive(true);
            warningLaser.transform.position = head.position;
            warningLaser.transform.rotation = Quaternion.Euler(0, 0, angle);

            UpdateWarningLaser(); 

            float timer = 0f;
            float blinkInterval = 0.15f;

            while (timer < warningTime)
            {
                warningLaser.SetActive(!warningLaser.activeSelf);

                yield return new WaitForSeconds(blinkInterval);

                timer += blinkInterval;
            }

            warningLaser.SetActive(false);
        }

        laser.gameObject.SetActive(true);
        bool playerStillThere = Vector2.Distance(player.position, lockedPosition) < 0.5f;

        float finalLength;

        if (playerStillThere)
        {
            finalLength = distanceToTarget;
        }
        else
        {
            finalLength = wallHit.collider != null ? wallHit.distance : distanceToTarget; 
        }

        currentLength = finalLength;
        UpdateLaser(currentLength);
        yield return new WaitForSeconds(stickTime);

        currentLength = 0;
        UpdateLaser(0);

        if (warningLaser != null)
            warningLaser.SetActive(false);

        laser.gameObject.SetActive(false);


      
    }

 

    void UpdateLaser(float length)
    {
        if (bodyRenderer != null)
        {
            bodyRenderer.size = new Vector2(length, bodyRenderer.size.y);
        }

        body.localPosition = new Vector3(length * 0.5f, 0, 0);
        tail.localPosition = new Vector3(length, 0, 0);

        if (bodyCollider != null)
        {
            float height = bodyCollider.size.y;
            float safeLength = Mathf.Max(length, 0.05f);
            bodyCollider.size = new Vector2(safeLength, height);
            bodyCollider.offset = new Vector2(length * 0f, 0);
        }
    }

    // ✅ Set độ dài cho warningLaser bằng localScale (vì nó chỉ là 1 sprite đơn, không có head/body/tail)
    private void UpdateWarningLaser()
    {
        if (warningRenderer == null) return;

        // 👉 hướng bắn (ví dụ theo rotation)
        Vector2 direction = transform.right;

        float maxDistance = 20f;

        // 👉 raycast chỉ hit tường
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            maxDistance,
            LayerMask.GetMask("Wall") // 👈 CHỈ tường
        );

        float length;

        if (hit.collider != null)
        {
            // 👉 dừng ở tường
            length = hit.distance;
        }
        else
        {
            // 👉 không có tường → full length
            length = maxDistance;
        }

        float safeLength = Mathf.Max(length, 0.05f);

        // 👉 scale laser
        Vector3 scale = warningLaser.transform.localScale;
        scale.x = safeLength / warningBaseWidth;
        warningLaser.transform.localScale = scale;
    }


}