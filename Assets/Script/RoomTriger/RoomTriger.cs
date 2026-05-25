using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class RoomTriger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bossHPBar;
    public GameObject boss;
    public AudioSource bossSound;
    public GameObject door;
    public GameObject introBoss;
    public GameObject player;
    public float introDuration = 2.5f;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {


            if (boss != null && door != null)
            {

                StartCoroutine(PlayIntro());
                boss.SetActive(true);
                bossHPBar.SetActive(true);
                bossSound.Play();
                door.SetActive(true);
                Debug.Log("HP Bar active: " + bossHPBar.activeSelf);
            }

            Debug.Log("Đã vào phòng Boss! Nhạc nổi lên!");

        }


        IEnumerator PlayIntro()
        {
            Time.timeScale = 0f;
            player.GetComponent<Player>().enabled = false;
            introBoss.SetActive(true);
            yield return new WaitForSecondsRealtime(introDuration);
            player.GetComponent<Player>().enabled = true;
            introBoss.SetActive(false);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }


    }

}
