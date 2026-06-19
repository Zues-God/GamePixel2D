using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class RoomTriger : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    private Animator animator;
    public GameObject bossHPBar, boss, introBoss, player;
    private List<GameObject> enemy = new List<GameObject>();
    public AudioSource bossSound;
    public BoxCollider2D spawArena;
    public int enemySpaw = 5;
    public float introDuration = 2.5f;
    private bool spawed = false;
    public Animator [] door;
    public Transform enemyParent;





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawed)
        {
            spawed = true;
            SpawEnemy();
            ActiveAllEnemy();

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
            float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            Vector2 spawnPos = new Vector2(randomX, randomY);
            GameObject randomEnemy = enemyPrefab[ Random.Range(0, enemyPrefab.Length)];
            GameObject e = Instantiate(randomEnemy, spawnPos, Quaternion.identity, enemyParent);
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
       
            if (!spawed) return;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length == 0)
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
                StartCoroutine(DisableAfterAnim(d));
            }

        }
        Debug.Log("Door Open!");
    }

    IEnumerator DisableAfterAnim(Animator d)
    {
        yield return new WaitForSeconds(0.3f); 

        d.gameObject.SetActive(false);
    }
    void ActiveAllEnemy()
    {
        foreach (Transform child in enemyParent)
        {
           Enemy e  = child.GetComponent<Enemy>();

            if (e != null)
            {
                e.ActivateEnemy();
            }
           
        }
    }
}