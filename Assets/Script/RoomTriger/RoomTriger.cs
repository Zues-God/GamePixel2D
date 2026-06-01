using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class RoomTriger : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public GameObject bossHPBar, boss, introBoss, player;
    private List<GameObject> enemy = new List<GameObject>();
    public AudioSource bossSound;
    public BoxCollider2D spawArena;
    public int enemySpaw = 5;
    public float introDuration = 2.5f;
    private bool spawed = false;
    public Animator [] door;
    public bool doorOpened = false;

   



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawed)
        {
            spawed = true;
            SpawEnemy();

            if (boss != null)
            {

                StartCoroutine(PlayIntro());
                boss.SetActive(true);
                bossHPBar.SetActive(true);
                bossSound.Play();
                Debug.Log("HP Bar active: " + bossHPBar.activeSelf);
                Debug.Log("Đã vào phòng Boss");
            }

            foreach (Animator d in door)
            {
                d.gameObject.SetActive(true);
                d.SetBool("isOpen", false);

            }


        }



    }
   


    void SpawEnemy()
    {
        Debug.Log("Enemy Prefab = " + enemyPrefab);
        Bounds bounds = spawArena.bounds;
        for (int i = 0; i < enemySpaw; i++)
        {
            float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float randomY = UnityEngine.Random.Range(bounds.max.y, bounds.min.y);
            Vector2 spawnPos = new Vector2(randomX, randomY);
            GameObject e = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
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
  
   

    void Update()
    {
        if (!spawed || doorOpened) return;
        enemy.RemoveAll(e => e == null);
        if (enemy.Count == 0)
        {
            OpenDoor();
        }
       
    }

    void OpenDoor()
    {
        foreach (Animator d in door) 
        {
            if (d != null)
            {
                d.SetBool("isOpen", true);
                d.gameObject.SetActive(false);
                doorOpened = true;  
            }

        }
        Debug.Log("Door Open!");
    }

}