using UnityEngine;

public class PlayerAttackOverride : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        // CHẶN spam melee logic bằng cách reset attack state gián tiếp
        if (Input.GetMouseButtonDown(0))
        {
            // Không cho melee “giữ state”
            player.Invoke("DisableHitBox", 0f);
        }
    }
}