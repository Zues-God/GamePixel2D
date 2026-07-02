using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private GameObject shieldOnPlayer;

    public void ThrowShield()
    {
        shieldOnPlayer.SetActive(false);
    }

    public void RecoverShield()
    {
        shieldOnPlayer.SetActive(true);
    }
}