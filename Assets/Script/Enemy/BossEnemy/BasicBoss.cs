using System.Collections;
using UnityEngine;

public class BasicBoss : Enemy
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Normal Bullet")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireCooldown = 2f;

    [Header("Circle Bullet")]
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float circleBulletSpeed = 5f;
    [SerializeField] private float circleCooldown = 4f;

    [Header("Heal")]
    [SerializeField] private float healAmount = 20f;
    [SerializeField] private float healInterval = 10f;

    private float nextFireTime;
    private float nextCircleTime;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(HealLoop());
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        if (Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireCooldown;
        }

        if (Time.time >= nextCircleTime)
        {
            CircleBullet();
            nextCircleTime = Time.time + circleCooldown;
        }
    }

    void FireBullet()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        EnemyBullet eb = bullet.AddComponent<EnemyBullet>();
        eb.SetMovementDirection(direction * bulletSpeed);
    }

    void CircleBullet()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            Vector3 dir = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0
            );

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            EnemyBullet eb = bullet.AddComponent<EnemyBullet>();
            eb.SetMovementDirection(dir * circleBulletSpeed);
        }
    }

    IEnumerator HealLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);

            if (currenHP <= 0) yield break;

            if (currenHP < maxHP)
            {
                currenHP = Mathf.Min(currenHP + healAmount, maxHP);
                UpdateHP();
            }
        }
    }
}