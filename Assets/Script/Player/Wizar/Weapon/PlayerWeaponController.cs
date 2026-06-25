//using UnityEngine;

//public class PlayerWeaponController : MonoBehaviour
//{
//    [SerializeField] private Weapon currentWeapon;
//    [SerializeField] private Animator animator;
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            //currentWeapon.Attack();
//            animator.SetBool("isCasting", true);
//            Debug.Log("Bắt đầu cast");
//        }

//        if (Input.GetMouseButtonUp(0))
//        {
//            animator.SetBool("isCasting", false);
//            Debug.Log("Kết thúc cast");
//        }
//    }

//}

using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Staff currentWeapon;
    //public 
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentWeapon.Attack();
        }

        if (Input.GetMouseButton(0))
        {
            currentWeapon.SetHolding(true);

        }

        if (Input.GetMouseButtonUp(0))
        {
            currentWeapon.SetHolding(false);

        }
    }
}