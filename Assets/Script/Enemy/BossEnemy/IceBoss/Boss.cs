using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Skill Settings")]
    [SerializeField] private float skillCooldown = 30f;
    [SerializeField] private float skillDuration = 10f;
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileDamage = 20f;
    private bool isUsingSkill = false;
    private Coroutine skillCoroutine;

    protected override void Start()
    {
        base.Start();

        skillCoroutine = StartCoroutine(SkillRoutine());
    }

    protected override void Update()
    {
        if (isUsingSkill)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        base.Update();
    }

    private IEnumerator SkillRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(skillCooldown);
            if (!isActive)
                continue;

            StartSkill();
            yield return new WaitForSecondsRealtime(skillDuration);

            EndSkill();
        }
    }

    private void StartSkill()
    {
        isUsingSkill = true;

        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isSkill", true);
    }

    private void EndSkill()
    {
        animator.SetBool("isSkill", false);
    }

    public void FinishSkill()
    {
        isUsingSkill = false;
    }

    protected override void AnimationDie()
    {
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
        }

        isUsingSkill = false;

        animator.SetBool("isSkill", false);

        base.AnimationDie();
    }

    public bool IsUsingSkill()
    {
        return isUsingSkill;
    }

    public void ShootProjectile()
    {
        if (player == null)
            return;

        if (projectilePrefab == null)
            return;

        Vector2 direction =
            (player.transform.position - firePoint.position).normalized;

        GameObject bullet =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity);

        BossButtletSkill projectile =
            bullet.GetComponent<BossButtletSkill>();

        if (projectile != null)
        {
            projectile.Initialize(
                direction,
                projectileSpeed,
                projectileDamage);
        }
    }

    protected override void DestroyEnemy()
    {
        if (GateSpawm.Instance != null)
        {
            GateSpawm.Instance.ShowPortal();
        }

        base.DestroyEnemy();
    }
}