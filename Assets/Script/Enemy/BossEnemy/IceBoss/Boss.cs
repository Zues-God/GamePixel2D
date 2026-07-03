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
        // ?ang dùng skill thì không cho Enemy AI ho?t ??ng
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
            // Ch? h?i chiêu
            yield return new WaitForSecondsRealtime(skillCooldown);

            // N?u Boss ch?a ???c kích ho?t thì ch?
            if (!isActive)
                continue;

            StartSkill();

            // Th?i gian dùng skill
            yield return new WaitForSecondsRealtime(skillDuration);

            EndSkill();
        }
    }

    private void StartSkill()
    {
        isUsingSkill = true;

        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isSkill", true);

        Debug.Log("========== BOSS START SKILL ==========");
    }

    private void EndSkill()
    {
        animator.SetBool("isSkill", false);

        Debug.Log("========== BOSS END SKILL ==========");
    }

    // Animation Event ? frame cu?i c?a Skill_End
    public void FinishSkill()
    {
        isUsingSkill = false;

        Debug.Log("========== FINISH SKILL ==========");
    }

    // Override hàm ch?t c?a Enemy
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

    // Animation Event trong Skill_Loop
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

        Debug.Log("Boss Shoot");
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