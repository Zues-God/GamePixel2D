using UnityEngine;

public class BossController : Enemy
{
    [Header("Skill")]
    [SerializeField] private float skillCooldown = 30f;
    [SerializeField] private float skillDuration = 10f;

    private bool isUsingSkill = false;

    private float nextSkillTime;
    private float skillStartTime;

    protected override void Start()
    {
        base.Start();

        nextSkillTime = Time.time + skillCooldown;
    }

    protected override void Update()
    {
        if (isUsingSkill)
        {
            CheckSkillDuration();
            return;
        }

        if (Time.time >= nextSkillTime)
        {
            StartSkill();
            return;
        }

        base.Update();
    }

    private void StartSkill()
    {
        isUsingSkill = true;

        skillStartTime = Time.time;

        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isSkill", true);

        Debug.Log("===== BOSS START SKILL =====");
    }

    private void CheckSkillDuration()
    {
        rb.linearVelocity = Vector2.zero;

        if (Time.time - skillStartTime >= skillDuration)
        {
            animator.SetBool("isSkill", false);

            Debug.Log("===== BOSS END LOOP =====");
        }
    }

    public void FinishSkill()
    {
        isUsingSkill = false;

        nextSkillTime = Time.time + skillCooldown;

        Debug.Log("===== BOSS FINISH SKILL =====");
    }

    public bool IsUsingSkill()
    {
        return isUsingSkill;
    }
}