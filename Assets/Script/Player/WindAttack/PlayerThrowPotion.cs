using UnityEngine;

public class PlayerThrowPotion : MonoBehaviour
{
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float cooldown = 0.5f;

    private Animator animator;
    private float lastThrowTime = -999f;
    private bool isThrowing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            !isThrowing &&
            Time.time >= lastThrowTime + cooldown)
        {
            isThrowing = true;
            lastThrowTime = Time.time;

            animator.SetTrigger("isAttack");
        }
    }

    // GỌI HÀM NÀY TRONG ANIMATION EVENT
    public void ThrowPotionEvent()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 direction = (mouseWorld - throwPoint.position).normalized;

        GameObject potion = Instantiate(
            potionPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        potion.GetComponent<PotionProjectile>().Init(direction);
    }

    // GỌI Ở CUỐI ANIMATION EVENT
    public void EndThrowPotionEvent()
    {
        isThrowing = false;
    }
}