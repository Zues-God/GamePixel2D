using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Player player = col.GetComponent<Player>();

            if (player != null)
            {
                player.PickupGun(gameObject);
            }
        }
    }
}