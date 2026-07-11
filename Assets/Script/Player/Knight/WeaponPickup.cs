using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Audio audioManager;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Player player = col.GetComponent<Player>();

            if (player != null)
            {
                audioManager.PlayTakeWeaponSound();
                player.PickupGun(gameObject);
            }
        }
    }
}