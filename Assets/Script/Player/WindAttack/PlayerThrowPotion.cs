using UnityEngine;

public class PlayerThrowPotion : Player 
{
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] public float cooldown = 0.5f;
    //[SerializeField] private Audio audioSource;

    public  Animator animator;
    public  float lastThrowTime = -999f;
    public bool isThrowing = false;


    protected override void Update()
    {
        base.Update();
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) &&
    //        !isThrowing &&
    //        Time.time >= lastThrowTime + cooldown)
    //    {
    //        isThrowing = true;
    //        lastThrowTime = Time.time;

    //        animator.SetTrigger("isAttack");
    //    }
    //}

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

        potion.GetComponent<PotionProjectile>().SetDirection(direction);
    }

   

    public void EndThrowPotionEvent()
    {
        isThrowing = false;
    }
}