using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float delayBeforeFire = 1f;
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float circleBulletSpeed = 5f;

    [Header("Skill Cooldowns")]
    [SerializeField] private float laserCooldown = 10f;
    [SerializeField] private float fireBulletCooldown = 4f;
    [SerializeField] private float circleBulletCooldown = 6f;

    [Header("Skill Loop Settings")]
    [SerializeField] private float skillCheckInterval = 0.5f; 
    [SerializeField] private float firstSkillDelay = 2f;

    [SerializeField] private Audio audioManager;

    [SerializeField] private float hpValue = 100f;
    [Header("Healing")]
    [SerializeField] private bool enableAutoHeal = false;
    [SerializeField] private float healAmount = 100f;
    [SerializeField] private float healInterval = 10f;
    private enum BossSkillType
    {
        Laser,
        FireBullet,
        CircleBullet
    }

    private Vector3 lastRecordedPlayerPosition;
    private bool isUsingSkill = false;

    private float nextLaserReadyTime;
    private float nextFireBulletReadyTime;
    private float nextCircleBulletReadyTime;
    private Coroutine healCoroutine;

    protected override void Start()
    {
        base.Start();

        laser.SetTarget(players);

        nextLaserReadyTime = Time.time + firstSkillDelay;
        nextFireBulletReadyTime = Time.time + firstSkillDelay;
        nextCircleBulletReadyTime = Time.time + firstSkillDelay;

        StartCoroutine(SkillLoop());

        if (enableAutoHeal && healAmount > 0f && healInterval > 0f)
        {
            healCoroutine = StartCoroutine(HealLoop());
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator SkillLoop()
    {
        while (true)
        {
            if (!isUsingSkill)
            {
                UseSkill();
            }

            yield return new WaitForSeconds(skillCheckInterval);
        }
    }

    private IEnumerator HealLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);

            if (currenHP <= 0f) yield break; 

            if (currenHP < maxHP)
            {
                HealEnemy(healAmount);
                if (audioManager != null)
                {
                    var mi = audioManager.GetType().GetMethod("PlayHealSound");
                    if (mi != null) mi.Invoke(audioManager, null);
                }
            }
        }
    }

    public void TriggerHealOnce()
    {
        if (healAmount <= 0f) return;

        HealEnemy(healAmount);
    }

    public void UseSkill()
    {
        if (isUsingSkill) return;

        List<BossSkillType> readySkills = new List<BossSkillType>();

        if (Time.time >= nextLaserReadyTime) readySkills.Add(BossSkillType.Laser);
        if (Time.time >= nextFireBulletReadyTime) readySkills.Add(BossSkillType.FireBullet);
        if (Time.time >= nextCircleBulletReadyTime) readySkills.Add(BossSkillType.CircleBullet);

        if (readySkills.Count == 0) return; 

        BossSkillType chosen = readySkills[UnityEngine.Random.Range(0, readySkills.Count)];
        StartCoroutine(ExecuteSkill(chosen));
    }

    private IEnumerator ExecuteSkill(BossSkillType skill)
    {
        isUsingSkill = true;

        switch (skill)
        {
            case BossSkillType.Laser:
                nextLaserReadyTime = Time.time + laserCooldown;
                yield return StartCoroutine(LaserSkillRoutine());
                break;

            case BossSkillType.FireBullet:
                nextFireBulletReadyTime = Time.time + fireBulletCooldown;
                yield return StartCoroutine(FireBulletSkillRoutine());
                break;

            case BossSkillType.CircleBullet:
                nextCircleBulletReadyTime = Time.time + circleBulletCooldown;
                CircleBullet(); 
                break;
        }

        isUsingSkill = false;
    }

    private IEnumerator LaserSkillRoutine()
    {
        audioManager.LaserBossSound();

        if (rbs != null) rbs.linearVelocity = Vector2.zero;
        if (movementScript != null) movementScript.enabled = false;
        animationLaser.SetActive(true);
        yield return StartCoroutine(laser.Fire());
        animationLaser.SetActive(false);

        if (movementScript != null) movementScript.enabled = true;
    }

    private IEnumerator FireBulletSkillRoutine()
    {
        if (player == null) yield break;

        lastRecordedPlayerPosition = player.transform.position;
        yield return new WaitForSeconds(delayBeforeFire);

        Vector3 direction = (lastRecordedPlayerPosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
        FireBullet(direction);
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


    private void HealEnemy(float hpAmount)
    {

        currenHP = Math.Min(currenHP + hpAmount, maxHP);
        UpdateHP();

    }

}