using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Skill Settings")]
    [SerializeField] private float skillCooldown = 30f;
    [SerializeField] private float skillDuration = 10f;

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
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_Start"));
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_Loop"));
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        base.Update();
        Debug.Log(animator.speed);
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
}