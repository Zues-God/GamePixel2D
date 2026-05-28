using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class RoomTriger : MonoBehaviour
{
    public GameObject bossHPBar;
    public GameObject boss;
    public AudioSource bossSound;
    public GameObject door;
    public GameObject introBoss;
    public GameObject player;
    public BoxCollider2D spawArena;
    public GameObject enemyPrefab;
    public int enemySpaw = 5;
    public float introDuration = 2.5f;
    private List<GameObject> enemy = new List<GameObject> ();
    private bool spawed = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawed)
        {

            if (boss != null && door != null)
            {

                StartCoroutine(PlayIntro());
                boss.SetActive(true);
                bossHPBar.SetActive(true);
                bossSound.Play();
                door.SetActive(true);
                SpawEnemy();
                Debug.Log("HP Bar active: " + bossHPBar.activeSelf);
            }

            Debug.Log("Đã vào phòng Boss! Nhạc nổi lên!");

        }
        

    }



    void SpawEnemy()
    {
       Bounds bounds = spawArena.bounds;
        for (int i = 0; i < enemySpaw; i++)
        {
            float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float randomY = UnityEngine.Random.Range(bounds.max.y, bounds.min.y);
            Vector2 spawnPos = new Vector2(randomX, randomY);
            GameObject e = Instantiate(enemyPrefab, spawnPos, quaternion.identity);
            enemy.Add(e);

        }

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