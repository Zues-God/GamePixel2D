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
        // Đang dùng skill
        if (isUsingSkill)
        {
            CheckSkillDuration();
            return;
        }

        // Ưu tiên dùng skill nếu đủ cooldown
        if (Time.time >= nextSkillTime)
        {
            StartSkill();
            return;
        }

        // Chưa dùng skill thì chạy AI bình thường
        base.Update();
    }

    private void StartSkill()
    {
        isUsingSkill = true;

        skillStartTime = Time.time;

        // Dừng di chuyển
        rb.linearVelocity = Vector2.zero;

        // Bắt đầu animation skill
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

    // Animation Event ở cuối Skill_End
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