using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxHpPlayer = 100f;
    [SerializeField] private Image hpBar;
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 200f;
    [SerializeField] private Image energyBar;
    [SerializeField] AudioSource stopAudio;
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;
    public GameObject hitBox;
    private float currentHpPlayer;
    private float currentEnergy;
    private bool isAttacking = false;
    [SerializeField] private float skillCost = 40f;
    [SerializeField] private float skillCooldown = 3f;
    [SerializeField] private GameObject skillKinght;
    private float lastSkillTime = -999f;
    public GameObject swordAnimation;
    [SerializeField] private Transform weapon;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHpPlayer = maxHpPlayer;
        currentEnergy = maxEnergy;
        hitBox.SetActive(false);
        UpdateEnergy();
        UpdateHP();

    }

    private void PlayerSkill()
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastSkillTime + skillCooldown && currentEnergy >= skillCost)
        {

            currentEnergy -= skillCost;
            UpdateEnergy();
            lastSkillTime = Time.time;
            Vector2 skillPos = transform.position;
            StartCoroutine(SkillRoutine());
        }
      
    }


    IEnumerator SkillRoutine()
    {
        Vector2 skillPos = transform.position;
        skillKinght.transform.position = skillPos;
        skillKinght.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        skillKinght.SetActive(false);
    }


    void Update()
    {

        MovePlayer();
        HandleFacingDirection();
        PlayerSkill();
        PlayerAttack();
    }
    private void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        if (playerInput.x < 0)
        {

            rbSprite.flipX = true;
            weapon.localScale = new Vector3(-1, 1, 1);

        }
        else if (playerInput.x > 0 )
        {

            rbSprite.flipX = false;
            weapon.localScale = new Vector3(1, 1, 1);

        }

        if (playerInput != Vector2.zero)          
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

    }

    private void HandleFacingDirection()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x < transform.position.x)
            {
                rbSprite.flipX = true;
                weapon.localScale = new Vector3(-1, 1, 1);

            }
            else
            {
                rbSprite.flipX = false;
                weapon.localScale = new Vector3(1, 1, 1);

            }
        }
        else
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput > 0)
            {
                rbSprite.flipX = false; 
            }
            else if (horizontalInput < 0)
            {
                rbSprite.flipX = true;  
            }
        }
    }


    public bool HasEnoughMana(float amount)
    {
        return currentEnergy >= amount;
    }

    public bool UseMana(float amount)
    {
        if (currentEnergy < amount)
            return false;

        currentEnergy -= amount;

        currentEnergy = Mathf.Max(currentEnergy, 0);

        UpdateEnergy();

        return true;
    }



    private void PlayerAttack()
    {

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("isAttack");
        }
    }


    public void EnableHitBox()
    {
        hitBox.SetActive(true);
    }


    public void DisableHitBox()
    {
        isAttacking = false;
        hitBox.SetActive(false);
    }


    private void Die()
    {

        stopAudio.Stop();
        Destroy(gameObject);
    }
    public void EnableAnimationSword()
    {
        swordAnimation.SetActive(true);
    }

    public void DisableAnimationSword()
    {
        swordAnimation.SetActive(false);
    }   



    public void TakeDamage(float damage)
    {
        currentHpPlayer -= damage;
        currentHpPlayer = Mathf.Max(currentHpPlayer, 0);
        UpdateHP();
        if (currentHpPlayer <= 0)
        {
            Die();
        }

    }

    public void Heal(float healValue)
    {
        if (currentHpPlayer < maxHpPlayer)
        {
            currentHpPlayer += healValue;
            currentHpPlayer = Mathf.Min(currentHpPlayer, maxHpPlayer);
            UpdateHP();
        }
    }
    private void UpdateHP()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHpPlayer / maxHpPlayer;
        }
    }
    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        UpdateEnergy();
    }
    private void UpdateEnergy()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }

}
