using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxHpPlayer = 100f;
    [SerializeField] private Image hpBar;
    [SerializeField] AudioSource stopAudio;
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;
    public GameObject hitBox;
    private float currentHpPlayer;
    private bool isAttacking = false;
    [SerializeField] private float skillCooldown = 3f;
    [SerializeField] private GameObject skillKinght;
    private float lastSkillTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 
    }

    void Start()
    {
        currentHpPlayer = maxHpPlayer;
        hitBox.SetActive(false);
        UpdateHP(); 

    }

    private void PlayerSkill()
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastSkillTime + skillCooldown)
        {
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
        PlayerSkill();
        PlayerAttack();
    }
    private void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        if (playerInput.x < 0) {

            rbSprite.flipX = true;

        } else if (playerInput.x > 0) {

            rbSprite.flipX= false;

        }

        if (playerInput != Vector2.zero)
        {
            animator.SetBool("isRun", true);
        }
        else {
            animator.SetBool("isRun", false);
        }

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
 


    public void TakeDamage(float damage)
    {
        currentHpPlayer -= damage;
        currentHpPlayer = Mathf.Max(currentHpPlayer, 0);
        UpdateHP();
        if(currentHpPlayer <= 0) {
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
        if (hpBar != null) { 
            hpBar.fillAmount = currentHpPlayer / maxHpPlayer;
        }
    }
  

}
