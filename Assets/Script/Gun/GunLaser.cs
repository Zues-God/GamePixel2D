using UnityEngine;

public class GunLaser : Gun
{
    [Header("Laser")]
    public GameObject laserObject;
    public float manaCostPerSecond;

    private bool isFiring = false;

    protected override void Shoot()
    {
        if (!canUse)
        {
            StopFiring();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            float manaThisFrame = manaCostPerSecond * Time.deltaTime;

            if (player.UseMana(manaThisFrame))
            {
                if (!isFiring)
                {
                    isFiring = true;
                    laserObject.SetActive(true);
                    audioManager.PlayShootSound();
                }
            }
            else
            {
                StopFiring();
            }
        }
        else
        {
            StopFiring();
        }
    }

    private void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;
            laserObject.SetActive(false);
        }
    }
}