using System;
using UnityEngine;

public class Gun : MonoBehaviour

{

    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    public string weaponName;
    public bool canUse = true;
    [SerializeField] private float manaCost = 5f;
    private Player player;
    [SerializeField] private Audio audioManager;



    public void SetPlayer(Player p)
    {
        player = p;
    }

    void Update()
    {

        RotateGun();
        Shoot();
    }

    void RotateGun()
    {
        if (!canUse) return;

        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width
         || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(1, -1, 1);
    }

    void Shoot()
    {
        if (!canUse) return;

        if (Input.GetMouseButton(0) && Time.time > nextShot)
        {
            if (!player.UseMana(manaCost))
            {
                return;
            }
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            audioManager.PlayShootSound();  
        }
    }


}
