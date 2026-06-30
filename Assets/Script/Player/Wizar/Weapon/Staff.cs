using UnityEngine;

public class Staff : Weapon
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [Header("Mana")]
    [SerializeField] private float manaCost = 5f;

    private Player player;
    private bool isHolding = false;
    private bool isCasting = false;
    private int queuedShots = 0;

    private float holdStartTime;
    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    public override void Attack()
    {
        queuedShots++;

        animator.SetBool("canExitAttack", false);

        if (!isCasting)
        {
            isCasting = true;
            animator.SetBool("isCasting", true);

        }
    }

    public override void SetHolding(bool holding)
    {
        isHolding = holding;

        Debug.Log("Holding: " + isHolding);
    }

    public void Shoot()
    {
        if (!player.UseMana(manaCost))
        {
            Debug.Log("Không đủ Mana");

            queuedShots = 0;

            isHolding = false;

            animator.SetBool("canExitAttack", true);
            animator.SetBool("isCasting", false);

            isCasting = false;

            return;
        }

        // 1. Luôn thực hiện bắn đạn khi animation gọi tới event này
        SpawnBullet();

        // 2. Nếu có đạn trong hàng đợi thì trừ đi 1
        // Dù đang giữ chuột hay không, đã bắn ra là phải trừ queue
        if (queuedShots > 0)
        {
            queuedShots--;
        }

        // 3. Kiểm tra điều kiện để dừng Attack
        // Chỉ dừng khi: KHÔNG còn giữ chuột VÀ đã xử lý hết đạn trong hàng đợi
        if (!isHolding && queuedShots <= 0)
        {
            // Kích hoạt cờ cho phép thoát attack
            animator.SetBool("canExitAttack", true);

            // QUAN TRỌNG: Ép isCasting về false ngay tại đây.
            // Điều này đảm bảo nếu Animator của bạn dựa vào "isCasting == false" để về Idle 
            // thì nó sẽ nhận được tín hiệu và thoát loop thành công, không bị chạy liên tục nữa.
            isCasting = false;
            animator.SetBool("isCasting", false);
        }
    }

    private void SpawnBullet()
    {
        Vector3 mousePos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0;

        Vector2 direction =
            (mousePos - firePoint.position).normalized;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity);

        FileBall bulletScript =
            bullet.GetComponent<FileBall>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        bullet.transform.rotation =
            Quaternion.Euler(0, 0, angle);


    }

    public void StartHold()
    {
        holdStartTime = Time.time;

        queuedShots++;

        animator.SetBool("canExitAttack", false);

        if (!isCasting)
        {
            isCasting = true;
            animator.SetBool("isCasting", true);
        }
    }

    public void StopHold()
    {
        // nếu chỉ click nhanh
        if (Time.time - holdStartTime < 0.5f)
        {
            Debug.Log("Click");
            return;
        }

        Debug.Log("Stop Hold");

        isHolding = false;
    }

    // Animation Event cuối Attack_End
    public void EndAttack()
    {
        Debug.Log("END ATTACK");

        isCasting = false;

        animator.SetBool(
            "isCasting",
            false
        );

        animator.SetBool(
            "canExitAttack",
            false
        );
    }
}