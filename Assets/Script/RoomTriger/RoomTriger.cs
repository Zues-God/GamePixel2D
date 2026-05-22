using UnityEngine;
using UnityEngine.UI;


public class RoomTriger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bossHPBar;
    public GameObject boss;
    public AudioSource bossSound;
    public GameObject door;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (boss != null && door != null)
            {
                boss.SetActive(true);
                bossHPBar.SetActive(true);
                bossSound.Play();
                door.SetActive(true);
                Debug.Log("HP Bar active: " + bossHPBar.activeSelf);
            }
            gameObject.SetActive(false);
            Debug.Log("Đã vào phòng Boss! Nhạc nổi lên!");
            Debug.Log(bossSound);
        }
        
    }
}
