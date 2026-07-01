using UnityEngine;

public class PlayerKnight : Player
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private PlayerShield playerShield;
    [SerializeField] private float shieldCooldown = 2f;
    private float lastShieldTime = -999f;


    protected override void Update()
    {
        base.Update(); 

        HandleShield(); 
    }


    private void HandleShield()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= lastShieldTime + shieldCooldown)
        {
            ThrowShield();
            lastShieldTime = Time.time;
        }
    }

  
    private void ThrowShield()
    {
        if (shieldPrefab == null || throwPoint == null) return;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouse - throwPoint.position);

        GameObject shield = Instantiate(shieldPrefab, throwPoint.position, Quaternion.identity);

        ShieldRicochet ricochet = shield.GetComponent<ShieldRicochet>();
        if (ricochet != null)
        {
            ricochet.Init(dir, transform);
        }

        if (playerShield != null)
        {
            playerShield.ThrowShield();
        }
    }
}