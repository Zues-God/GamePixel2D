using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private BossLaser laser;
    [SerializeField] private Transform players, firePoint;
    [SerializeField] private Rigidbody2D rbs;
    [SerializeField] private MonoBehaviour movementScript;
    [SerializeField] private GameObject animationLaser, bulletRefabs;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float recordInterval = 2f; 
    [SerializeField] private float delayBeforeFire = 1f; 
    const int bulletCount = 8;
    [SerializeField] private float circleBulletSpeed = 5f;
    private Vector3 lastRecordedPlayerPosition;

    protected override void Start()
    {
        base.Start();

        laser.SetTarget(players);

        StartCoroutine(LaserLoop());
        StartCoroutine(DelayedFireLoop());
    }
    protected override void Update()
    {
        base.Update();
     
    }

    private IEnumerator DelayedFireLoop()
    {
        while (true)
        {
            if (player != null)
            {
                lastRecordedPlayerPosition = player.transform.position;
                yield return new WaitForSeconds(delayBeforeFire);

                Vector3 direction = (lastRecordedPlayerPosition - firePoint.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
                FireBullet(direction);
            }

            yield return new WaitForSeconds(recordInterval);
        }
    }

    private IEnumerator FireLaser()
    {
        if (rbs != null) rbs.linearVelocity = Vector2.zero;

        if (movementScript != null) movementScript.enabled = false;

        yield return StartCoroutine(laser.Fire());

        if (movementScript != null) movementScript.enabled = true;

    }

    private IEnumerator LaserLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            animationLaser.SetActive(true);
            yield return StartCoroutine(FireLaser());
            animationLaser.SetActive(false);

        }
    }
    private void FireBullet(Vector3 directionToTarget)
    {
        GameObject bullet = Instantiate(bulletRefabs, firePoint.position, Quaternion.identity);
        EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
        enemyBullet.SetMovementDirection(directionToTarget * bulletSpeed);
    }

    public void CircleBullet()
    {
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(bulletRefabs, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(bulletDirection * circleBulletSpeed);    


        }

    }


    

}
