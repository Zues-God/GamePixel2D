using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("--- THÔNG SỐ CƠ BẢN (CORE STATS) ---")]
    public int level = 1;
    public float currentExp = 0f;
    public float maxExp = 100f;
    public float elementalMastery = 0f;

    [Header("--- HP (Sinh Lực) ---")]
    public float maxHP = 100f;
    public float currentHP;
    public Slider hpSlider;

    [Header("--- MP (Năng Lượng) ---")]
    public float maxMP = 50f;
    public float currentMP;
    public float mpRegenRate = 2f;
    public Slider mpSlider;

    [Header("--- THỂ LỰC & DI CHUYỂN ---")]
    public float moveSpeed = 5f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 15f;
    public Slider staminaSlider;

    [Header("--- LƯỚT (DASH) ---")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float dashCost = 20f;
    private bool isDashing;
    private bool canDash = true;

    [Header("--- TẤN CÔNG (COMBAT) ---")]
    public float attackDamage = 15f;
    public float attackRange = 1.2f;
    public LayerMask enemyLayers;

    [Header("--- HIỆU ỨNG THỞ (BREATHING) ---")]
    public float breatheSpeed = 3f;
    public float breatheAmount = 0.03f;
    private Vector3 startScale;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down; // Đang đóng vai trò là animFacing
    private Animator anim;

    // [THÊM MỚI] Biến lưu lại input của frame trước đó
    private Vector2 lastInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startScale = transform.localScale;

        currentHP = maxHP;
        currentMP = maxMP;
        currentStamina = maxStamina;

        UpdateUI(hpSlider, currentHP, maxHP);
        UpdateUI(mpSlider, currentMP, maxMP);
        UpdateUI(staminaSlider, currentStamina, maxStamina);
    }

    void Update()
    {
        if (isDashing) return;

        HandleMovementInput();
        UpdateAnimation();
        HandleRegeneration();
        HandleBreathing();

        if (Input.GetKeyDown(KeyCode.J)) Attack();

        if (Input.GetKeyDown(KeyCode.Space) && canDash && currentStamina >= dashCost && movement != Vector2.zero)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.H)) TakeDamage(10f);
        if (Input.GetKeyDown(KeyCode.X)) GainExp(50f);
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // [SỬA ĐỔI] Áp dụng logic tách biệt vector di chuyển và vector hướng mặt
    void HandleMovementInput()
    {
        // 1. Lấy input thô (không normalize) để xử lý logic hướng mặt
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 2. Logic chốt hướng mặt vào biến lastMoveDirection
        if (rawInput != Vector2.zero)
        {
            if (lastInput == Vector2.zero)
            {
                lastMoveDirection = rawInput;
                if (rawInput.x != 0 && rawInput.y != 0)
                {
                    lastMoveDirection.y = 0;
                }
            }
            else if (rawInput != lastInput)
            {
                if (lastMoveDirection.x != 0 && rawInput.x == 0)
                {
                    lastMoveDirection = new Vector2(0, rawInput.y);
                }
                else if (lastMoveDirection.y != 0 && rawInput.y == 0)
                {
                    lastMoveDirection = new Vector2(rawInput.x, 0);
                }
            }
        }

        // Lưu lại input cho frame sau
        lastInput = rawInput;

        // 3. Gán vector di chuyển vật lý (có normalize để đi chéo không bị nhanh)
        movement = rawInput.normalized;
    }

    void UpdateAnimation()
    {
        anim.SetFloat("MoveX", lastMoveDirection.x);
        anim.SetFloat("MoveY", lastMoveDirection.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void HandleBreathing()
    {
        if (movement == Vector2.zero)
        {
            anim.speed = 0;
            float newY = startScale.y + Mathf.Sin(Time.time * breatheSpeed) * breatheAmount;
            transform.localScale = new Vector3(startScale.x, newY, startScale.z);
        }
        else
        {
            anim.speed = 1;
            transform.localScale = startScale;
        }
    }

    void HandleRegeneration()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            if (staminaSlider != null) staminaSlider.value = currentStamina;
        }

        if (currentMP < maxMP)
        {
            currentMP += mpRegenRate * Time.deltaTime;
            currentMP = Mathf.Clamp(currentMP, 0, maxMP);
            if (mpSlider != null) mpSlider.value = currentMP;
        }
    }

    void Attack()
    {
        Vector2 attackPos = rb.position + (lastMoveDirection * 0.5f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Tạm thời comment dòng dưới nếu bạn chưa tạo class Enemy để tránh lỗi báo đỏ trong console
            // enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        currentStamina -= dashCost;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (hpSlider != null) hpSlider.value = currentHP;

        Debug.Log("Player bị đau! Máu còn: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("GAME OVER! Bị cook lần 2!");
        }
    }

    public void GainExp(float expAmount)
    {
        currentExp += expAmount;
        Debug.Log("Nhận " + expAmount + " EXP. Đang có: " + currentExp + "/" + maxExp);

        if (currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentExp -= maxExp;
        maxExp = maxExp * 1.5f;

        maxHP += 20f;
        maxMP += 10f;
        attackDamage += 5f;

        currentHP = maxHP;
        currentMP = maxMP;

        UpdateUI(hpSlider, currentHP, maxHP);
        UpdateUI(mpSlider, currentMP, maxMP);

        Debug.Log("LÊN CẤP! BẠN ĐÃ ĐẠT CẤP " + level);
    }

    void UpdateUI(Slider slider, float currentValue, float maxValue)
    {
        if (slider != null)
        {
            slider.maxValue = maxValue;
            slider.value = currentValue;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (rb == null) return;
        Vector2 attackPos = (Vector2)transform.position + (lastMoveDirection * 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}