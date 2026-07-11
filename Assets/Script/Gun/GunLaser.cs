using UnityEngine;

public class GunLaser : Gun
{
    [Header("Laser")]
    [SerializeField] private GameObject laserPrefab;   
    public float manaCostPerSecond;
    private GameObject laserInstance;   
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
                    StartFiring();
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

    private void StartFiring()
    {
        isFiring = true;

        laserInstance = Instantiate(laserPrefab, firePos.position, firePos.rotation, firePos);

        audioManager.PlayShootSound();
    }

    private void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;

            if (laserInstance != null)
            {
                Destroy(laserInstance);
                laserInstance = null;
            }
        }
    }

    private void OnDisable()
    {
        StopFiring();
    }
}