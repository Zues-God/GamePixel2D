using UnityEngine;

public class RangeAttack : Enemy
{
    [SerializeField] private GameObject buttletPrefab;

    [SerializeField] private Transform firePoint;

    public void ShootProjectile()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(buttletPrefab,
                                        firePoint.position,
                                        Quaternion.identity);

        bullet.GetComponent<Buttlet>().Init(player.transform.position);
    }
}