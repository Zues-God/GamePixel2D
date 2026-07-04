using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxHpPlayer = 100f;
    [SerializeField] private Image hpBar;
    [SerializeField] private float maxEnergy = 200f;
    [SerializeField] private Image energyBar;
    [SerializeField] private float hitCooldown = 0.1f;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform backHolder;
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private Transform weapon;
    [SerializeField] private float skillCost = 40f;
    [SerializeField] private float skillCooldown = 3f;
    [SerializeField] private GameObject skillPlayer;
    [SerializeField] private Audio audioManager;
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;
    public GameObject hitBox;
    private float currentHpPlayer;
    private float currentEnergy;
    private float lastSkillTime = -999f;
    public GameObject animationWeapon;
    private float lastHitTime;
    private GameObject currentWeapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    protected virtual void Start()
    {
        currentWeapon = weapon1;
        weapon1.transform.SetParent(weaponHolder);
        currentHpPlayer = maxHpPlayer;
        currentEnergy = maxEnergy;
    }
    public void PickupGun(GameObject gunObj)
    {
       
        audioManager.PlayTakeWeaponSound();
        gunObj.transform.SetParent(backHolder);

        gunObj.transform.localPosition = Vector3.zero;
        gunObj.transform.localRotation = Quaternion.identity;

        weapon2 = gunObj;

        if (weapon1 != null)
        {
            weapon1.SetActive(false);

        }
        currentWeapon = weapon2;
        Gun gun = weapon2.GetComponent<Gun>();
        gun.SetPlayer(this);
        if (gun != null)
            gun.canUse = true;
        WeaponPickup pickup = weapon2.GetComponent<WeaponPickup>();
        if (pickup != null)
            Destroy(pickup);
    }

    public void SwapWeapon()
    {
        if (weapon1 == null || weapon2 == null) return;

        if (currentWeapon == weapon1)
        {
        
            currentWeapon = weapon2;

            weapon1.SetActive(false);
            weapon2.SetActive(true);
        }
        else
        {
         
            currentWeapon = weapon1;

            weapon2.SetActive(false);
            weapon1.SetActive(true);
        }
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
            audioManager.PlaySkillSound();
        }

    }


    IEnumerator SkillRoutine()
    {
        Vector2 skillPos = transform.position;
        skillPlayer.transform.position = skillPos;
        skillPlayer.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        skillPlayer.SetActive(false);
    }

    protected virtual void Update()
    {

      
        ChangeWeapon();
        MovePlayer();
        HandleFacingDirection();
        PlayerSkill();
        PlayerAttack();
       
    }


    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            audioManager.PlayChangeWeaponSound();
            if (weapon2.transform.childCount == 0)
            {
                Debug.LogWarning("No weapon to swap to.");
                return;

            }
            SwapWeapon();  
        }
    }

    private void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        if (playerInput.x < 0)
        {
            rbSprite.flipX = true;

            if (weapon != null)
                weapon.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerInput.x > 0)
        {
            rbSprite.flipX = false;

            if (weapon != null)
                weapon.transform.localScale = new Vector3(1, 1, 1);
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
                weapon.transform.localScale = new Vector3(-1, 1, 1);

            }
            else
            {
                rbSprite.flipX = false;
                weapon.transform.localScale = new Vector3(1, 1, 1);

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


    void PlayerAttack()
    {
        if (currentWeapon == weapon2) return;

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttack");
            audioManager.PlayAttackSound();
        }
    }


    public void EnableHitBox()
    {
        hitBox.SetActive(true);
    }


    public void DisableHitBox()
    {
        hitBox.SetActive(false);
    }


    private void Die()
    {
        Destroy(gameObject);
    }
    public void EnableAnimationSword()
    {
        animationWeapon.SetActive(true);
    }

    public void DisableAnimationSword()
    {
        animationWeapon.SetActive(false);
    }


    public void TakeDamage(float damage)
    {
        if (Time.time < lastHitTime + hitCooldown) return;
        lastHitTime = Time.time;
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
