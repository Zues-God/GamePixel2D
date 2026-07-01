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